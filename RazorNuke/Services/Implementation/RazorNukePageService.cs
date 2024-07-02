using Microsoft.EntityFrameworkCore;
using RazorNuke.DbContext;
using RazorNuke.Models;
using RazorNuke.Models.ViewModels;
using RSecurityBackend.Models.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RazorNuke.Services.Implementation
{
    public class RazorNukePageService : IRazorNukePageService
    {

        /// <summary>
        /// add new page
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> AddAsync(Guid userId, RazorNukePage page)
        {
            try
            {
                page.Title = page.Title.Trim();
                if (string.IsNullOrEmpty(page.UrlSlug))
                    page.UrlSlug = "";
                page.UrlSlug = page.UrlSlug.Trim();
                if (string.IsNullOrEmpty(page.Title))
                    return new RServiceResult<RazorNukePage?>(null, "Title could not be empty");
                if (page.UrlSlug.Contains("/"))
                    return new RServiceResult<RazorNukePage?>(null, "UrlSulg should not contain any /");
                page.CreateDate = DateTime.Now;
                page.LastModified = DateTime.Now;
                page.CreateUserId = userId;
                if(string.IsNullOrEmpty(page.TitleInMenu))
                {
                    page.TitleInMenu = page.Title;
                }
                page.TitleInMenu = page.TitleInMenu.Trim();
                string urlPrefix = "/";
                string titlePrefix = "";
                if (page.ParentId != null)
                {
                    var parentPage = await _context.Pages.AsNoTracking().Where(p => p.Id == page.ParentId).SingleAsync();
                    urlPrefix = $"{parentPage.FullUrl}/";
                    titlePrefix = $"{parentPage.FullTitle} » ";
                }
                page.FullUrl = $"{urlPrefix}{page.UrlSlug}";
                if (await _context.Pages.AsNoTracking().AnyAsync(a => a.FullUrl == page.FullUrl))
                    return new RServiceResult<RazorNukePage?>(null, "Duplicated full url.");
                page.FullTitle = $"{titlePrefix}{page.Title}";
                page.PlainText = _ExtractText(page.HtmlText);
                _context.Add(page);
                await _context.SaveChangesAsync();

                _cachedMenu = null;
                await _RebuildSitemapAsync();

                return new RServiceResult<RazorNukePage?>(page);
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage?>(null, exp.ToString());
            }
        }

        /// <summary>
        /// update page
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> UpdateAsync(Guid userId, RazorNukePage page)
        {
            try
            {
                var dbPage = await _context.Pages.Where(p => p.Id == page.Id).SingleAsync();
                RazorNukePageSnapshot snapshot = new RazorNukePageSnapshot()
                {
                    Page = dbPage,
                    MadeObsoleteByUserId = userId,
                    RecordDate = DateTime.Now,
                    Note = "",
                    PageOrder = dbPage.PageOrder,
                    Published = dbPage.Published,
                    TitleInMenu = dbPage.TitleInMenu,
                    Title = dbPage.Title,
                    FullTitle = dbPage.FullTitle,
                    UrlSlug = dbPage.UrlSlug,
                    FullUrl = dbPage.FullUrl,
                    HtmlText = dbPage.HtmlText,
                    PlainText = dbPage.PlainText,
                };
                _context.Add(snapshot);

                page.Title = page.Title.Trim();
                if (string.IsNullOrEmpty(page.UrlSlug))
                    page.UrlSlug = "";
                page.UrlSlug = page.UrlSlug.Trim();
                page.TitleInMenu = page.TitleInMenu.Trim();
                
                page.CreateDate = dbPage.CreateDate;
                page.CreateUserId = dbPage.CreateUserId;
                if(page.ParentId != dbPage.ParentId)
                {
                    string urlPrefix = "/";
                    string titlePrefix = "";
                    if (page.ParentId != null)
                    {
                        var parentPage = await _context.Pages.AsNoTracking().Where(p => p.Id == page.ParentId).SingleAsync();
                        urlPrefix = $"{parentPage.FullUrl}/";
                        titlePrefix = $"{parentPage.FullTitle} » ";
                    }
                    page.FullUrl = $"{urlPrefix}{page.UrlSlug}";
                    page.FullTitle = $"{titlePrefix}{page.Title}";

                    if (await _context.Pages.AsNoTracking().AnyAsync(a => a.FullUrl == page.FullUrl && a.Id != page.Id))
                        return new RServiceResult<RazorNukePage?>(null, "Duplicated full url.");

                }
                else
                {
                    page.FullUrl = dbPage.FullUrl;
                    page.FullTitle = dbPage.FullTitle;
                }
                page.LastModified = DateTime.Now;
                page.PlainText = _ExtractText(page.HtmlText);
                _context.Entry(dbPage).CurrentValues.SetValues(page);
                _context.Update(dbPage);
                await _context.SaveChangesAsync();

                await _UpdateChildren(dbPage);

                _cachedMenu = null;
                await _RebuildSitemapAsync();

                return new RServiceResult<RazorNukePage?>(dbPage);
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage?>(null, exp.ToString());
            }
        }

        private async Task _UpdateChildren(RazorNukePage parentPage)
        {
            var children = await _context.Pages.Where(p => p.ParentId == parentPage.Id).ToListAsync();
            if (!children.Any()) return;
            var urlPrefix = $"{parentPage.FullUrl}/";
            var titlePrefix = $"{parentPage.FullTitle} » ";

            foreach (var page in children)
            {
                page.FullUrl = $"{urlPrefix}{page.UrlSlug}";
                page.FullTitle = $"{titlePrefix}{page.Title}";
            }
            _context.UpdateRange(children);
            await _context.SaveChangesAsync();
            foreach (var page in children)
            {
                await _UpdateChildren(page);
            }
        }

        private static string _ExtractText(string html)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            string s = reg.Replace(html, " ");
            s = HttpUtility.HtmlDecode(s);
            return s;
        }

        /// <summary>
        /// delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (await _context.Pages.Where(p => p.ParentId == id).AnyAsync())
                    return new RServiceResult<bool>(false, "Page has children.");
                var snapshots = await _context.PageSnapshots.Where(p => p.PageId == id).ToArrayAsync();
                _context.RemoveRange(snapshots);
                var dbPage = await _context.Pages.Where(p => p.Id == id).SingleAsync();
                _context.Remove(dbPage);
                await _context.SaveChangesAsync();
                _cachedMenu = null;
                await _RebuildSitemapAsync();
                return new RServiceResult<bool>(true);
            }
            catch (Exception exp)
            {
                return new RServiceResult<bool>(false, exp.ToString());
            }
        }

        /// <summary>
        /// get page children
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage[]?>> GetPageChildrenAsync(int? parentId, bool onlyPublished)
        {
            try
            {
                return new RServiceResult<RazorNukePage[]?>
                    (
                    await _context.Pages.AsNoTracking()
                        .Where(p => p.ParentId == parentId && (onlyPublished == false || p.Published == true))
                        .OrderBy(p => p.PageOrder)
                        .ToArrayAsync()
                    );
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage[]?>(null, exp.ToString());
            }
        }

        private static RazorNukeMenuItem? _cachedMenu = null;

        /// <summary>
        /// get menu
        /// </summary>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukeMenuItem?>> GetMenuAsync()
        {
            try
            {
                if(_cachedMenu != null )
                {
                    return new RServiceResult<RazorNukeMenuItem?>(_cachedMenu);
                }
                var res1 = await _context.Pages.AsNoTracking()
                                .Where(p => p.Published)
                                .Select(p => new RazorNukeMenuItem()
                                {
                                    Id = p.Id,
                                    PageOrder = p.PageOrder,
                                    TitleInMenu = p.TitleInMenu,
                                    FullUrl = p.FullUrl,
                                }).ToArrayAsync();

                RazorNukeMenuItem topLevel = new RazorNukeMenuItem()
                {
                    Id = 0
                };
                _BuildMenu(res1, topLevel, null);
                _cachedMenu = topLevel;
                return new RServiceResult<RazorNukeMenuItem?>(_cachedMenu);               

            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukeMenuItem?>(null, exp.ToString());
            }
        }

        private void _BuildMenu(RazorNukeMenuItem[] src, RazorNukeMenuItem parent, int? parentId)
        {
            parent.Children = src.Where(p => p.ParentId == parentId).OrderBy(p => p.PageOrder).ToArray();
            foreach (var child in parent.Children)
            {
                _BuildMenu(src, child, child.Id);
            }
        }

        /// <summary>
        /// get page by url
        /// </summary>
        /// <param name="fullUrl"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> GetByFullUrlAsync(string fullUrl, bool onlyPublished)
        {
            try
            {
                if (fullUrl.IndexOf('?') != -1)
                {
                    fullUrl = fullUrl.Substring(0, fullUrl.IndexOf('?'));
                }

                // /hafez/ => /hafez :
                if (fullUrl != "/" && fullUrl.LastIndexOf('/') == fullUrl.Length - 1)
                {
                    fullUrl = fullUrl.Substring(0, fullUrl.Length - 1);
                }

                fullUrl = fullUrl.Replace("//", "/"); //duplicated slashes would be merged
                return new RServiceResult<RazorNukePage?>(await _context.Pages.AsNoTracking().Where(p => p.FullUrl == fullUrl && (onlyPublished == false || p.Published == true)).FirstOrDefaultAsync());
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage?>(null, exp.ToString());
            }
        }

        /// <summary>
        /// get page by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> GetAsync(int id)
        {
            try
            {
                return new RServiceResult<RazorNukePage?>(await _context.Pages.AsNoTracking().Where(p => p.Id == id).SingleOrDefaultAsync());
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage?>(null, exp.ToString());
            }
        }

        private async Task _RebuildSitemapAsync()
        {
            try
            {
                string xmlSitemap = Configuration.GetSection("RazorNuke")["SitemapAbosultePathOnDisk"] ?? "";
                if (string.IsNullOrEmpty(xmlSitemap))
                    return;
                WriteSitemap(xmlSitemap, await _context.Pages.AsNoTracking().Where(p => p.Published).OrderBy(p => p.ParentId).ThenBy(p => p.PageOrder).Select(p => p.FullUrl).ToListAsync());
            }
            catch
            {
                //ignore
            }
        }

        private void WriteSitemap(string filePath, List<string> urls)
        {
            var baseUrl = Configuration.GetSection("RazorNuke")["BaseUrlNoLeadingSlash"];
            if (baseUrl == null) return;
            if (File.Exists(filePath))
                File.Delete(filePath);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";

            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(ns + "urlset",
                new XAttribute(XNamespace.Xmlns + "xsi", xsiNs),
                new XAttribute(xsiNs + "schemaLocation",
                    "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"),
                from url in urls
                select new XElement(ns + "url",
                    new XElement(ns + "loc", $"{baseUrl}{url}"))
                )
            );

            xDoc.Save(filePath);
        }


        protected readonly RDbContext _context;
        protected readonly IConfiguration Configuration;
        public RazorNukePageService(RDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
    }
}
