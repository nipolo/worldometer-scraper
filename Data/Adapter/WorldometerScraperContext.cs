using BF.WorldometerScraper.Data.States;

using Microsoft.EntityFrameworkCore;

namespace BF.WorldometerScraper.Data.Adapter
{
    public class WorldometerScraperContext : DbContext
    {
        public WorldometerScraperContext(DbContextOptions<WorldometerScraperContext> options)
            : base(options)
        {
        }

        public DbSet<CountryDailyCases> CountryDailyCases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryDailyCases>()
                .HasKey(a => new { a.CountryName, a.Time });
        }
    }
}
