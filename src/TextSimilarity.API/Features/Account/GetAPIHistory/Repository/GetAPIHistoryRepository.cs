using Microsoft.EntityFrameworkCore;
using QueryFilter.Extensions;
using QueryFilter.Models;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.Extensions;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIHistory.DTO;

namespace TextSimilarity.API.Features.Account.GetAPIHistory.Repository
{
    public interface IGetAPIHistoryRepository
    {
        Task<(IEnumerable<APIHistoryItem> items, int rowCount)> GetAPIHistoryAsync(long userId, Filter queryFilter, CancellationToken cancellationToken = default);
    }
    public class GetAPIHistoryRepository : IGetAPIHistoryRepository
    {

        private readonly AppDataContext _db;
        public GetAPIHistoryRepository(AppDataContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<APIHistoryItem> items, int rowCount)> GetAPIHistoryAsync(long userId, Filter queryFilter, CancellationToken cancellationToken = default)
        {
            var query = _db.RequestResponseLogs
                .AsNoTracking()
                .Where(r => r.UserId == userId && r.RequestSource == RequestSourse.API.ToString());

            var q = query
                .ApplyFilter(queryFilter)
                .Select(r => new APIHistoryItem
                {
                    Duration = r.Duration,
                    Request = r.Request,
                    RequestDate = r.RequestDate,
                    Response = r.Response,
                    ResponseCode = r.ResponseCode
                }).ToQueryString();

            var items = await query
                .ApplyFilter(queryFilter)
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
                .CountAsync(cancellationToken);

            return (items, rowCount);
        }
    }
}
