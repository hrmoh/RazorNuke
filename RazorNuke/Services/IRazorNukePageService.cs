using RazorNuke.Models;
using RSecurityBackend.Models.Generic;

namespace RazorNuke.Services
{
    public interface IRazorNukePageService
    {
        /// <summary>
        /// get page children
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage[]?>> GetPageChildrenAsync(int? parentId);
        /// <summary>
        /// get page by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> GetByUrlAsync(string url);
    }
}
