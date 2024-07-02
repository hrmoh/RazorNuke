using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models.ViewModels;

namespace RazorNuke.Pages
{
    public class _MenuPartModel : PageModel
    {
        public RazorNukeMenuItem  MenuItem { get; set; }
        public string CurrentPageFullUrl { get; set; }
    }
}
