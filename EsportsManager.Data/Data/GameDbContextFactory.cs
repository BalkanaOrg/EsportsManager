using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace EsportsManager.EsportsManager.Data.Data
{
    public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        public GameDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();

            // For design-time tooling like migrations, use a fixed path
            string dbPath = "esports_manager_design.db"; // any path you prefer
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new GameDbContext(dbPath);
        }
    }
}
