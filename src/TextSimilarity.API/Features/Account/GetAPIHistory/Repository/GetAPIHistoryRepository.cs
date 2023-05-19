using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIHistory.DTO;

namespace TextSimilarity.API.Features.Account.GetAPIHistory.Repository
{
    public interface IGetAPIHistoryRepository
    {
        Task<IEnumerable<APIHistoryItem>> GetAPIHistoryAsync(long userId, CancellationToken cancellationToken = default);
    }
    public class GetAPIHistoryRepository : IGetAPIHistoryRepository
    {

        private readonly AppDataContext _db;
        public GetAPIHistoryRepository(AppDataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<APIHistoryItem>> GetAPIHistoryAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _db.RequestResponseLogs
                .AsNoTracking()
                .Where(r => r.UserId == userId && r.RequestSource == RequestSourse.API.ToString())
                .Select(r => new APIHistoryItem
                {
                    Duration = r.Duration,
                    Request = r.Request,
                    RequestDate = r.RequestDate,
                    Response = r.Response,
                    ResponseCode = r.ResponseCode
                })
                .ToListAsync(cancellationToken);
        }
    }
}
