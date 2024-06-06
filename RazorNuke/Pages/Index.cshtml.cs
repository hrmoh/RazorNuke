using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;

namespace RazorNuke.Pages
{
    public class IndexModel : PageModel
    {
        public RazorNukePage? RazorNukePage { get; set; }
        public string FatalError { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync()
        {
            var res = await _pagesService.GetByUrlAsync(Request.Path);
            if (!string.IsNullOrEmpty(res.ExceptionString))
            {
                FatalError = res.ExceptionString;
            }
            if (res.Result == null)
            {
                return NotFound();
            }
            RazorNukePage = res.Result;
            return Page();
        }



        protected readonly IRazorNukePageService _pagesService;
        public IndexModel(IRazorNukePageService pagesService)
        {
            _pagesService = pagesService;
        }
    }
}
