using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;

namespace RazorNuke.Pages
{
    public class IndexModel : PageModel
    {
        public RazorNukePage? CurrentPage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Language"] = Configuration.GetSection("RazorNuke")["Language"];
            var direction = Configuration.GetSection("RazorNuke")["Direction"];
            ViewData["Direction"] = direction;
            var siteName = Configuration.GetSection("RazorNuke")["SiteName"];
            var sep = Configuration.GetSection("RazorNuke")["TitlePartsSeparator"];
            if (direction == "rtl")
            {
                ViewData["SiteTitlePart"] = $"{siteName} {sep} ";
            }
            else
            {
                ViewData["SiteTitlePart"] = $" {sep} {siteName}";
            }

            ViewData["LoggedIn"] = !string.IsNullOrEmpty(Request.Cookies["Token"]);
            var resMenuTopLevelPages = await _pagesService.GetPageChildrenAsync(null);
            if (!string.IsNullOrEmpty(resMenuTopLevelPages.ExceptionString))
            {
                ViewData["FatalError"] = resMenuTopLevelPages.ExceptionString;
                return Page();
            }
            ViewData["MenuTopLevelPages"] = resMenuTopLevelPages.Result;
            if (resMenuTopLevelPages.Result!.Length == 0)
            {
                return Page();
            }


            var resCurrentPage = await _pagesService.GetByUrlAsync(Request.Path);
            if (!string.IsNullOrEmpty(resCurrentPage.ExceptionString))
            {
                ViewData["FatalError"] = resCurrentPage.ExceptionString;
                return Page();
            }
            if (resCurrentPage.Result == null)
            {
                return NotFound();
            }
            CurrentPage = resCurrentPage.Result;

            ViewData["Id"] = CurrentPage.Id;

            return Page();
        }

        protected readonly IRazorNukePageService _pagesService;
        protected readonly IConfiguration Configuration;
        public IndexModel(IRazorNukePageService pagesService, IConfiguration configuration)
        {
            _pagesService = pagesService;
            Configuration = configuration;
            
        }
    }
}
