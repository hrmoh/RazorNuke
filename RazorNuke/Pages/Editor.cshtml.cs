using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;

namespace RazorNuke.Pages
{
    public class EditorModel : PageModel
    {
        [BindProperty]
        public RazorNukePage CurrentPage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["MenuTopLevelPages"] = new RazorNukePage[] { };
            if (Guid.TryParse(Request.Cookies["UserId"], out Guid userId))
            {
                if (userId != Guid.Empty)
                {
                    if (string.IsNullOrEmpty(Request.Query["id"]))
                    {
                        CurrentPage = new RazorNukePage()
                        {
                            Id = 0,
                            PageOrder = 0,
                            Title = "",
                            FullTitle = "",
                            TitleInMenu = "",
                            UrlSlug = "/",
                            FullUrl = "/",
                            HtmlText = "",
                            PlainText = "",
                            Published = false,
                            CreateDate = DateTime.Now,
                            LastModified = DateTime.Now,
                        };
                    }
                    else
                    {
                        var resCurrentPage = await _pagesService.GetAsync(int.Parse(Request.Query["id"]));
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
                    }
                    return Page();
                }
            }
            return Forbid();
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Guid.TryParse(Request.Cookies["UserId"], out Guid userId))
            {
                if (userId != Guid.Empty)
                {
                    if (CurrentPage.Id == 0)
                    {
                        var res = await _pagesService.AddAsync(userId, CurrentPage);
                        if(!string.IsNullOrEmpty(res.ExceptionString))
                        {
                            return BadRequest(res.ExceptionString);
                        }
                        CurrentPage = res.Result!;
                    }
                    else
                    {
                        var res = await _pagesService.UpdateAsync(userId, CurrentPage);
                        if (!string.IsNullOrEmpty(res.ExceptionString))
                        {
                            return BadRequest(res.ExceptionString);
                        }
                        CurrentPage = res.Result!;

                    }
                    return Page();
                }
            }
            return Forbid();
        }

        protected readonly IRazorNukePageService _pagesService;
        public EditorModel(IRazorNukePageService pagesService)
        {
            _pagesService = pagesService;
        }
    }
}
