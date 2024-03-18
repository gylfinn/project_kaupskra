using Microsoft.EntityFrameworkCore;

namespace project_kaupskra.Models
{
    public class FasteignakaupContext : DbContext
    {
        public FasteignakaupContext(DbContextOptions<FasteignakaupContext> options)
            : base(options)
        {
        }

        public DbSet<Fasteignakaup>? Fasteignakaup { get; set; }
    }

}
