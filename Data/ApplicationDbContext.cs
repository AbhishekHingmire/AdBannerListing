using Microsoft.EntityFrameworkCore;

namespace AdBannerListings.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdBannerListings.Models.Banner> Banners { get; set; }
    }
}