using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BF.WorldometerScraper.Data.Adapter
{
    public class WorldometerScraperContextFactory : IDesignTimeDbContextFactory<WorldometerScraperContext>
    {
        public WorldometerScraperContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorldometerScraperContext>();

            var dbFolder = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources"
                );
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            var sqliteDBFile = Path.Combine(dbFolder, "WorldometerScraper.db");
            optionsBuilder.UseSqlite($"Data Source={sqliteDBFile}");

            return new WorldometerScraperContext(optionsBuilder.Options);
        }
    }
}
