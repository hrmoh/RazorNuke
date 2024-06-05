using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RSecurityBackend.Services;

namespace RazorNuke.Pages
{
    public class IndexModel : PageModel
    {
        public int UserCount { get; set; }

        protected readonly IAppUserService _usersService;

        public IndexModel(IAppUserService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var res = await _usersService.GetAllUsersInformation(new RSecurityBackend.Models.Generic.PagingParameterModel() { PageNumber = 1, PageSize = 20 }, string.Empty, string.Empty);
            if(string.IsNullOrEmpty(res.ExceptionString))
            {
                UserCount = res.Result.Items.Length + 1;
            }
            return Page();
        }
    }
}
