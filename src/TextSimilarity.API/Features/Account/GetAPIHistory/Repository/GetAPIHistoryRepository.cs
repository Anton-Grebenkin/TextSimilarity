using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.Extensions;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIHistory.DTO;

namespace TextSimilarity.API.Features.Account.GetAPIHistory.Repository
{
    public interface IGetAPIHistoryRepository
    {
        Task<(IEnumerable<APIHistoryItem> items, int rowCount)> GetAPIHistoryAsync(long userId, int start, int size, ColumnSort[] sorts, CancellationToken cancellationToken = default);
    }
    public class GetAPIHistoryRepository : IGetAPIHistoryRepository
    {

        private readonly AppDataContext _db;
        public GetAPIHistoryRepository(AppDataContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<APIHistoryItem> items, int rowCount)> GetAPIHistoryAsync(long userId, int start, int size, ColumnSort[] sorts, CancellationToken cancellationToken = default)
        {
            var query = _db.RequestResponseLogs
                .AsNoTracking()
                .Where(r => r.UserId == userId && r.RequestSource == RequestSourse.API.ToString());

            var items = await query
                .OrderBySorts(sorts)
                .Skip(start)
                .Take(size)
                .Select(r => new APIHistoryItem
                {
                    Duration = r.Duration,
                    Request = r.Request,
                    RequestDate = r.RequestDate,
                    Response = r.Response,
                    ResponseCode = r.ResponseCode
                })
                .ToListAsync(cancellationToken);

            var rowCount = await query
                .CountAsync();

            return (items, rowCount);
        }
    }
}
