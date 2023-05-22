using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Common.DataAccess
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
            Init();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<APIKey> APIKeys { get; set; }

        public DbSet<RequestResponseLog> RequestResponseLogs { get; set; }


        private void Init()
        {
            Database.EnsureCreated();
        }
    }
}
