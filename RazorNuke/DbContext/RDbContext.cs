using Microsoft.EntityFrameworkCore;
using RazorNuke.Models;
using RSecurityBackend.DbContext;
using RSecurityBackend.Models.Auth.Db;

namespace RazorNuke.DbContext
{
    /// <summary>
    /// Main Db Context
    /// </summary>
    public class RDbContext : RSecurityDbContext<RAppUser, RAppRole, Guid>
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="options"></param>
        public RDbContext(DbContextOptions options) : base(options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json")
                   .Build();
            if (bool.Parse(configuration["DatabaseMigrate"] ?? false.ToString()))
            {
                Database.Migrate();
            }
        }

        /// <summary>
        /// indexing and ...
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// pages
        /// </summary>
        public DbSet<RazorNukePage> Pages { get; set; }

        /// <summary>
        /// page snapshots
        /// </summary>
        public DbSet<RazorNukePageSnapshot> PageSnapshots { get; set; }
    }
}
