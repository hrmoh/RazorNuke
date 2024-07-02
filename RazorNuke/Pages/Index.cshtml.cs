using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            ViewData["SiteName"]  = siteName;
            
            bool loggedIn = !string.IsNullOrEmpty(Request.Cookies["Token"]);
            ViewData["LoggedIn"] = loggedIn;
            var resMenu = await _pagesService.GetMenuAsync();
            if (!string.IsNullOrEmpty(resMenu.ExceptionString))
            {
                ViewData["FatalError"] = resMenu.ExceptionString;
                return Page();
            }

            var menu = resMenu.Result;

            ViewData["FooterItems"] = Configuration.GetSection("FooterItems").Get<string[]>();


            if (menu.Length == 0)
            {
                return Page();
            }



            var resCurrentPage = await _pagesService.GetByFullUrlAsync(Request.Path, !loggedIn);
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
            var sep = Configuration.GetSection("RazorNuke")["TitlePartsSeparator"];
            if (CurrentPage.FullUrl == "/")
            {
                ViewData["Title"] = siteName;
            }
            else
            if (direction == "rtl")
            {
                ViewData["Title"] = $"{siteName} {sep} {CurrentPage.FullTitle}";
            }
            else
            {
                ViewData["Title"] = $"{CurrentPage.FullTitle} {sep} {siteName}";
            }

            foreach (var menuTopLevelPage in menu)
            {
                menuTopLevelPage.Selected = menuTopLevelPage.FullUrl == CurrentPage.FullUrl;
            }

            ViewData["Menu"] = menu;


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
