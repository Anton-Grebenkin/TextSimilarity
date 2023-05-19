using Microsoft.EntityFrameworkCore;

namespace TextSimilarity.API.Common.DataAccess
{
    public interface IAppDataContextProvider
    {
        AppDataContext GetContext();
    }
    public class AppDataContextProvider : IAppDataContextProvider
    {
        private readonly DbContextOptions<AppDataContext> _options;
        public AppDataContextProvider(string? connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            _options = optionsBuilder
                   .UseSqlServer(connectionString)
                   .Options;
        }

        public AppDataContext GetContext()
        {
            return new AppDataContext(_options);
        }
    }
}
