using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Features.Account.GenerateAPIKey.Repository
{
    public interface IGenerateAPIKeyRepository
    {
        Task AddAPIKeyAsync(long userId, string apiKey, CancellationToken cancellationToken = default);
        Task<bool> ActiveAPIKeyExistsAsync(long userId, CancellationToken cancellationToken = default);
    }
    public class GenerateAPIKeyRepository : IGenerateAPIKeyRepository
    {
        private readonly AppDataContext _db;
        public GenerateAPIKeyRepository(AppDataContext db)
        {
            _db = db;
        }

        public Task<bool> ActiveAPIKeyExistsAsync(long userId, CancellationToken cancellationToken = default)
        {
            return _db.APIKeys.Where(a => a.UserId == userId && a.IsActive).AnyAsync(cancellationToken);
        }

        public Task AddAPIKeyAsync(long userId, string apiKey, CancellationToken cancellationToken = default)
        {
            var apiKeyEntity = new APIKey
            {
                UserId = userId,
                Value = apiKey,
                IsActive = true
            };

            _db.APIKeys.AddAsync(apiKeyEntity, cancellationToken);
            _db.SaveChangesAsync(cancellationToken);

            return Task.CompletedTask;
        }
    }
}
