using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project_kaupskra.Models;

namespace project_kaupskra.Data
{
    public class KaupsamningurDbContext : IdentityDbContext<AppUser>
    {
        public KaupsamningurDbContext(DbContextOptions<KaupsamningurDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kaupsamningur>? Fasteignakaup { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }

}
