using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorNuke.Models;

namespace RazorNuke.Pages
{
    public class EditorModel : PageModel
    {
        public IActionResult OnGet()
        {
            ViewData["MenuTopLevelPages"] = new RazorNukePage[] { };
            return Page();
        }
    }
}
