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
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> AddAsync(Guid userId, RazorNukePage page);

        /// <summary>
        /// update page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> UpdateAsync(Guid userId, RazorNukePage page);

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
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage[]?>> GetPageChildrenAsync(int? parentId, bool onlyPublished);

        /// <summary>
        /// get page by url
        /// </summary>
        /// <param name="fullUrl"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> GetByFullUrlAsync(string fullUrl, bool onlyPublished);

        /// <summary>
        /// get page by id
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<RServiceResult<RazorNukePage?>> GetAsync(int id);
    }
}
