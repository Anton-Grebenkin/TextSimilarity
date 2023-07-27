using FluentResults;
using MediatR;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIKey.Repository;

namespace TextSimilarity.API.Features.Account.GetAPIKey.UseCase
{
    [Authorize(RequestSourse.UI)]
    public record GetAPIKeyRequest() : IRequest<Result<GetAPIKeyResponse>>;
    public record GetAPIKeyResponse(string ApiKey);
    public class GetAPIKeyInteractor : IRequestHandler<GetAPIKeyRequest, Result<GetAPIKeyResponse>>
    {
        private readonly IGetAPIKeyRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        public GetAPIKeyInteractor(IGetAPIKeyRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        public async Task<Result<GetAPIKeyResponse>> Handle(GetAPIKeyRequest request, CancellationToken cancellationToken)
        {
            var userInfo = _currentUserService.GetCurrentUser();

            var apiKey = await _repository.GetAPIKeyAsync(userInfo.UserId, cancellationToken);

            if (apiKey == null)
                return Result.Fail(GetAPIKeyErrors.APIKeyNotFound());

            return Result.Ok(new GetAPIKeyResponse(apiKey.Value));
        }
    }
}
