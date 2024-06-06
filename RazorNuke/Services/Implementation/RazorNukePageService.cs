using Microsoft.EntityFrameworkCore;
using RazorNuke.DbContext;
using RazorNuke.Models;
using RSecurityBackend.Models.Generic;

namespace RazorNuke.Services.Implementation
{
    public class RazorNukePageService : IRazorNukePageService
    {

        /// <summary>
        /// add new page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> AddAsync(RazorNukePage page)
        {
            try
            {
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
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> UpdateAsync(RazorNukePage page)
        {
            try
            {
                var dbPage = await _context.Pages.Where(p => p.Id == page.Id).SingleAsync();
                _context.Entry(dbPage).CurrentValues.SetValues(dbPage);
                _context.Update(dbPage);
                await _context.SaveChangesAsync();
                return new RServiceResult<RazorNukePage?>(dbPage);
            }
            catch (Exception exp)
            {
                return new RServiceResult<RazorNukePage?>(null, exp.ToString());
            }
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
                if (url.LastIndexOf('/') == url.Length - 1)
                {
                    url = url.Substring(0, url.Length - 1);
                }

                url = url.Replace("//", "/"); //duplicated slashes would be merged
                return new RServiceResult<RazorNukePage?>(await _context.Pages.AsNoTracking().Where(p => p.FullUrl == url).SingleOrDefaultAsync());
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
