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

            return Page();
        }



        protected readonly IRazorNukePageService _pagesService;
        public IndexModel(IRazorNukePageService pagesService)
        {
            _pagesService = pagesService;
        }
    }
}
