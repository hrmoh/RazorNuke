using RazorNuke.DbContext;

namespace RazorNuke.Services.Implementation
{
    public class RazorNukePageService
    {


        protected readonly RDbContext _context;
        public RazorNukePageService(RDbContext context)
        {
            _context = context;
        }
    }
}
