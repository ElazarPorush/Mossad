using Microsoft.EntityFrameworkCore;
using MossadAPI.Models;

namespace MossadAPI.Data
{
    public class MossadDBContext: DbContext
    {
        public MossadDBContext(DbContextOptions<MossadDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new
                DbContextOptionsBuilder(), connectionString).Options;
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Location> Locations { get; set; }

    }
}
