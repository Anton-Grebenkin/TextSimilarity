using FluentResults;
using MediatR;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIHistory.DTO;
using TextSimilarity.API.Features.Account.GetAPIHistory.Repository;

namespace TextSimilarity.API.Features.Account.GetAPIHistory.UseCase
{
    [Authorize(RequestSourse.UI)]
    public record GetAPIHistoryRequest(int Start, int Size, ColumnSort[] Sorts) : IRequest<Result<GetAPIHistoryResponse>>;
    public record GetAPIHistoryResponse(IEnumerable<APIHistoryItem> items, int RowCount);
    public class GetAPIHistoryInteractor : IRequestHandler<GetAPIHistoryRequest, Result<GetAPIHistoryResponse>>
    {
        private readonly IGetAPIHistoryRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        public GetAPIHistoryInteractor(IGetAPIHistoryRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        public async Task<Result<GetAPIHistoryResponse>> Handle(GetAPIHistoryRequest request, CancellationToken cancellationToken)
        {
            var userInfo = _currentUserService.GetCurrentUser();

            var histoty = await _repository.GetAPIHistoryAsync(userInfo.UserId, request.Start, request.Size, request.Sorts, cancellationToken);

            return Result.Ok(new GetAPIHistoryResponse(histoty.items, histoty.rowCount));
        }
    }
}
