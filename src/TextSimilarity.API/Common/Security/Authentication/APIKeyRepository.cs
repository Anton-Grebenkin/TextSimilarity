using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Common.Security.Authentication
{
    public interface IAPIKeyRepository
    {
        APIKey GetAPIKeyAsync(string apiKey);
    }
    public class APIKeyRepository : IAPIKeyRepository
    {
        private readonly AppDataContext _db;
        public APIKeyRepository(AppDataContext db)
        {
            _db = db;
        }
        public APIKey? GetAPIKeyAsync(string apiKey)
        {
            return _db.APIKeys
                    .Where(a => a.Value == apiKey)
                    .FirstOrDefault();
        }
    }
}
