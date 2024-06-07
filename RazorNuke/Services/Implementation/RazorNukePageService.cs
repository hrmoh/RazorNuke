using Microsoft.AspNetCore.Authorization;
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
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RServiceResult<RazorNukePage?>> AddAsync(Guid userId, RazorNukePage page)
        {
            try
            {
                page.CreateDate = DateTime.Now;
                page.LastModified = DateTime.Now;
                page.CreateUserId = userId;
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
                var dbPage = await _context.Pages.Where(p => p.Id == page.Id).SingleAsync();
                _context.Entry(dbPage).CurrentValues.SetValues(dbPage);
                dbPage.LastModified = DateTime.Now;
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

        protected IAuthorizationRequirement _authorization;
        public RazorNukePageService(RDbContext context, IAuthorizationRequirement authorization)
        {
            _context = context;
            _authorization = authorization;
        }
    }
}
