using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;
using RazorNuke.Models.ViewModels;
using RSecurityBackend.Models.Auth.ViewModels;
using RSecurityBackend.Models.Generic;
using RSecurityBackend.Services;


namespace RazorNuke.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public IActionResult OnGet()
        {
            ViewData["Language"] = Configuration.GetSection("RazorNuke")["Language"];
            var direction = Configuration.GetSection("RazorNuke")["Direction"];
            ViewData["Direction"] = direction;
            ViewData["FooterItems"] = Configuration.GetSection("FooterItems").Get<string[]>();
            ViewData["Menu"] = new RazorNukeMenuItem[] { };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            LoginViewModel.ClientAppName = "RazorNuke";
            LoginViewModel.Language = "fa-IR";

            string clientIPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            RServiceResult<LoggedOnUserModel> res = await _service.Login(LoginViewModel, clientIPAddress);
            if (res.Result == null)
            {
                ViewData["FatalError"] = res.ExceptionString;
                return Page();
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
            Response.Cookies.Append("Name", $"{loggedOnUser.User.FirstName} {loggedOnUser.User.SurName}", cookieOption);
            Response.Cookies.Append("NickName", $"{loggedOnUser.User.NickName}", cookieOption);

            return Redirect("/");
        }


        protected IAppUserService _service;

        /// <summary>
        /// for client IP resolution
        /// </summary>
        protected IHttpContextAccessor _httpContextAccessor;

        protected readonly IConfiguration Configuration;

        public LoginModel(IAppUserService service, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _service = service;
            LoginViewModel = new LoginViewModel();
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

    }

}
