using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;

namespace TextSimilarity.API.Features.Account.RevokeAPIKey.Repository
{
    public interface IRevokeAPIKeyRepository
    {
        Task<bool> ActiveAPIKeyExistsAsync(long userId, CancellationToken cancellationToken = default);
        Task RevokeActiveAPIKeyAsync (long userId, CancellationToken cancellationToken = default);
    }
    public class RevokeAPIKeyRepository : IRevokeAPIKeyRepository
    {
        private readonly AppDataContext _db;
        public RevokeAPIKeyRepository(AppDataContext db)
        {
            _db = db;
        }

        public Task<bool> ActiveAPIKeyExistsAsync(long userId, CancellationToken cancellationToken = default)
        {
            return _db.APIKeys.Where(a => a.UserId == userId && a.IsActive).AnyAsync(cancellationToken);
        }

        public async Task RevokeActiveAPIKeyAsync(long userId, CancellationToken cancellationToken = default)
        {
            var apiKeyEntity = await _db.APIKeys.Where(a => a.UserId == userId && a.IsActive).FirstAsync(cancellationToken);

            apiKeyEntity.IsActive = false;

            //_db.APIKeys.Update(apiKeyEntity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
