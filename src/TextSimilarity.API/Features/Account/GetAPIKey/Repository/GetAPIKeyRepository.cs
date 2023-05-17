using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Features.Account.GetAPIKey.Repository
{
    public interface IGetAPIKeyRepository
    {
        Task<APIKey> GetAPIKeyAsync(long userId, CancellationToken cancellationToken = default);
    }
    public class GetAPIKeyRepository : IGetAPIKeyRepository
    {
        private readonly AppDataContext _db;
        public GetAPIKeyRepository(AppDataContext db)
        {
            _db = db;
        }
        public async Task<APIKey?> GetAPIKeyAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _db.APIKeys.FirstOrDefaultAsync(a => a.UserId == userId && a.IsActive, cancellationToken);
        }
    }
}
