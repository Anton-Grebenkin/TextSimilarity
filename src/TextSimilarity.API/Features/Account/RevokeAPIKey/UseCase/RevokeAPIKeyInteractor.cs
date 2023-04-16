using FluentResults;
using MediatR;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.RevokeAPIKey.Repository;

namespace TextSimilarity.API.Features.Account.RevokeAPIKey.UseCase
{
    [Authorize(RequestSourse.UI)]
    public record RevokeAPIKeyRequest() : IRequest<Result>;
    public class RevokeAPIKeyInteractor : IRequestHandler<RevokeAPIKeyRequest, Result>
    {
        private readonly IRevokeAPIKeyRepository _revokeAPIKeyRepository;
        private readonly ICurrentUserService _currentUserService;
        public RevokeAPIKeyInteractor(IRevokeAPIKeyRepository revokeAPIKeyRepository, ICurrentUserService currentUserService)
        {
            _revokeAPIKeyRepository = revokeAPIKeyRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(RevokeAPIKeyRequest request, CancellationToken cancellationToken)
        {
            var userInfo = _currentUserService.GetCurrentUser();

            var activeAPIKeyExists = await _revokeAPIKeyRepository.ActiveAPIKeyExistsAsync(userInfo.UserId, cancellationToken);
            if (!activeAPIKeyExists)
                return Result.Fail(RevokeAPIKeyErrors.ActiveAPIKeyNotFound());

            await _revokeAPIKeyRepository.RevokeActiveAPIKeyAsync(userInfo.UserId, cancellationToken);

            return Result.Ok();
        }
    }
}
