using RazorNuke.Models;
using RSecurityBackend.Models.Generic;

namespace RazorNuke.Services
{
    public interface IRazorNukePageService
    {
        /// <summary>
        /// add new page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> AddAsync(RazorNukePage page);

        /// <summary>
        /// update page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> UpdateAsync(RazorNukePage page);

        /// <summary>
        /// delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RServiceResult<bool>> DeleteAsync(int id);

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
