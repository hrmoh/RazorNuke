using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Services;
using RSecurityBackend.Models.Auth.ViewModels;
using RSecurityBackend.Models.Generic;
using RSecurityBackend.Models.Image;
using RSecurityBackend.Services;
using System.Security.Claims;

namespace RazorNuke.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class EditorModel : PageModel
    {
        [BindProperty]
        public RazorNukePage CurrentPage { get; set; }
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

            ViewData["MenuTopLevelPages"] = new RazorNukePage[] { };

            if(string.IsNullOrEmpty(Request.Cookies["Token"]))
            {
                ViewData["FatalError"] = "Please login!";
                return Page();
            }
            ClaimsPrincipal? principal;

            try
            {
                principal = _userService.GetPrincipalFromToken(Request.Cookies["Token"], false);
            }
            catch
            {
                principal = _userService.GetPrincipalFromToken(Request.Cookies["Token"], true);
                string clientIPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                RServiceResult<LoggedOnUserModel> res = await _userService.ReLogin(Guid.Parse(Request.Cookies["SessionId"]), clientIPAddress);
                if (res.Result == null)
                {
                    return Redirect("/Login");
                }

                LoggedOnUserModel loggedOnUser = res.Result;

                var cookieOption = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(365),
                };

                Response.Cookies.Append("UserId", loggedOnUser.User.Id.ToString(), cookieOption);
                Response.Cookies.Append("SessionId", loggedOnUser.SessionId.ToString(), cookieOption);
                Response.Cookies.Append("Token", loggedOnUser.Token, cookieOption);
                Response.Cookies.Append("Username", loggedOnUser.User.Username, cookieOption);
                Response.Cookies.Append("Name", $"{loggedOnUser.User.FirstName} {loggedOnUser.User.SureName}", cookieOption);
                Response.Cookies.Append("NickName", $"{loggedOnUser.User.NickName}", cookieOption);
                principal = _userService.GetPrincipalFromToken(loggedOnUser.Token, false);

            }

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
                    UrlSlug = "",
                    FullUrl = "",
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

            ViewData["MenuTopLevelPages"] = new RazorNukePage[] { };

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

        public async Task<IActionResult> OnPostUploadImageAsync(IFormFile file)
        {
            try
            {
                RServiceResult<RImage> image = await _pictureFileService.Add(file, null, file.FileName, "Images", true);
                if (!string.IsNullOrEmpty(image.ExceptionString))
                {
                    return new BadRequestObjectResult(image.ExceptionString);
                }
                image = await _pictureFileService.Store(image.Result);
                if (!string.IsNullOrEmpty(image.ExceptionString))
                {
                    return new BadRequestObjectResult(image.ExceptionString);
                }
                return new OkObjectResult(
                    new
                    {
                        Location = $"/images/{image.Result.Id}{Path.GetExtension(file.FileName)}"
                    }
                    );
            }
            catch (Exception exp)
            {
                return new BadRequestObjectResult(exp.ToString());
            }

        }

        protected readonly IRazorNukePageService _service;
        protected readonly IConfiguration Configuration;
        protected IAppUserService _userService;
        protected readonly IImageFileService _pictureFileService;
        protected IHttpContextAccessor _httpContextAccessor;
        public EditorModel(IConfiguration configuration, IRazorNukePageService pagesService, IAppUserService userService, IHttpContextAccessor httpContextAccessor, IImageFileService pictureFileService)
               
        {
            _service = pagesService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _pictureFileService = pictureFileService;
            Configuration = configuration;
        }
    }
}
