using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RSecurityBackend.Controllers;
using RSecurityBackend.Services;

namespace RazorNuke.Controllers
{
    [Produces("application/json")]
    [Route("images")]
    public class ImageController : RImageControllerBase
    {
        public ImageController(IImageFileService pictureFileService, IMemoryCache memoryCache) : base(pictureFileService, memoryCache)
        {
        }

    }
}
