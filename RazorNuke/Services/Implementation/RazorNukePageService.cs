using Microsoft.EntityFrameworkCore;
using RazorNuke.DbContext;
using RazorNuke.Models;
using RSecurityBackend.Models.Generic;

namespace RazorNuke.Services.Implementation
{
    public class RazorNukePageService : IRazorNukePageService
    {

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
