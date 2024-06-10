using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;
using RSecurityBackend.Services;

namespace RazorNuke.Pages
{
    public class EditorModel : PageModel
    {
        [BindProperty]
        public RazorNukePage CurrentPage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["MenuTopLevelPages"] = new RazorNukePage[] { };

            if(string.IsNullOrEmpty(Request.Cookies["Token"]))
            {
                ViewData["FatalError"] = "Please login!";
                return Page();
            }
            var principal = _userService.GetPrincipalFromToken(Request.Cookies["Token"], false);
            Guid userId = new Guid(principal.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            Guid sessionId = new Guid(principal.Claims.FirstOrDefault(c => c.Type == "SessionId").Value);
            var resSession = await _userService.SessionExists(userId, sessionId);
            if (!string.IsNullOrEmpty(resSession.ExceptionString))
            {
                ViewData["FatalError"] = resSession.ExceptionString;
                return Page();
            }
            if(resSession.Result == false)
            {
                ViewData["FatalError"] = "User session does not exist.";
                return Page();
            }

            if (string.IsNullOrEmpty(Request.Query["id"]))
            {
                CurrentPage = new RazorNukePage()
                {
                    Id = 0,
                    PageOrder = 0,
                    Title = "صفحهٔ جدید",
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
                var resCurrentPage = await _service.GetAsync(int.Parse(Request.Query["id"]));
                if (!string.IsNullOrEmpty(resCurrentPage.ExceptionString))
                {
                    ViewData["FatalError"] = resCurrentPage.ExceptionString;
                    return Page();
                }
                if (resCurrentPage.Result == null)
                {
                    ViewData["FatalError"] = "Page not found!";
                    return Page();
                }
                CurrentPage = resCurrentPage.Result;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Request.Cookies["Token"]))
            {
                ViewData["FatalError"] = "Please login";
                return Page();
            }
            var principal = _userService.GetPrincipalFromToken(Request.Cookies["Token"], false);
            Guid userId = new Guid(principal.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            Guid sessionId = new Guid(principal.Claims.FirstOrDefault(c => c.Type == "SessionId").Value);
            var resSession = await _userService.SessionExists(userId, sessionId);
            if (!string.IsNullOrEmpty(resSession.ExceptionString))
            {
                ViewData["FatalError"] = resSession.ExceptionString;
                return Page();
            }
            if (resSession.Result == false)
            {
                ViewData["FatalError"] = "User session does not exist.";
                return Page();
            }

            if (CurrentPage.Id == 0)
            {
                var res = await _service.AddAsync(userId, CurrentPage);
                if (!string.IsNullOrEmpty(res.ExceptionString))
                {
                    return BadRequest(res.ExceptionString);
                }
                CurrentPage = res.Result!;
            }
            else
            {
                var res = await _service.UpdateAsync(userId, CurrentPage);
                if (!string.IsNullOrEmpty(res.ExceptionString))
                {
                    return BadRequest(res.ExceptionString);
                }
                CurrentPage = res.Result!;

            }
            return Page();
        }

        protected readonly IRazorNukePageService _service;

        protected IAppUserService _userService;
        public EditorModel(IRazorNukePageService pagesService, IAppUserService userService)
        {
            _service = pagesService;
            _userService = userService;
        }
    }
}
