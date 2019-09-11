using Microsoft.EntityFrameworkCore;

namespace CasoNaranjitoSac.Models
{
    public class AnalyticsContext : DbContext
    {
        public AnalyticsContext(DbContextOptions<AnalyticsContext> options)
            : base(options)
        {
        }

        public DbSet<Analytics> AnalyticsItems { get; set; }
    }
}