using Microsoft.EntityFrameworkCore;
using RazorNuke.DbContext;
using RazorNuke.Models;
using RSecurityBackend.Models.Generic;
using System.Text.RegularExpressions;
using System.Web;

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
                page.Title = page.Title.Trim();
                if (string.IsNullOrEmpty(page.UrlSlug))
                    page.UrlSlug = "";
                page.UrlSlug = page.UrlSlug.Trim();
                page.TitleInMenu = page.TitleInMenu.Trim();
                var dbPage = await _context.Pages.Where(p => p.Id == page.Id).SingleAsync();
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
                var dbPage = await _context.Pages.Where(p => p.Id == id).SingleAsync();
                _context.Remove(dbPage);
                await _context.SaveChangesAsync();
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
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage[]?>> GetPageChildrenAsync(int? parentId)
        {
            try
            {
                return new RServiceResult<RazorNukePage[]?>(await _context.Pages.AsNoTracking().Where(p => p.ParentId == parentId).OrderBy(p => p.PageOrder).ToArrayAsync());
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage[]?>(null, exp.ToString());
            }
        }

        /// <summary>
        /// get page by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> GetByUrlAsync(string url)
        {
            try
            {
                if (url.IndexOf('?') != -1)
                {
                    url = url.Substring(0, url.IndexOf('?'));
                }

                // /hafez/ => /hafez :
                if (url != "/" && url.LastIndexOf('/') == url.Length - 1)
                {
                    url = url.Substring(0, url.Length - 1);
                }

                url = url.Replace("//", "/"); //duplicated slashes would be merged
                return new RServiceResult<RazorNukePage?>(await _context.Pages.AsNoTracking().Where(p => p.FullUrl == url).FirstOrDefaultAsync());
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

        protected readonly RDbContext _context;
        public RazorNukePageService(RDbContext context)
        {
            _context = context;
        }
    }
}
