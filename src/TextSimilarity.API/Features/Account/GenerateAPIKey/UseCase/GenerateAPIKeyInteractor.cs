using FluentResults;
using MediatR;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GenerateAPIKey.Repository;

namespace TextSimilarity.API.Features.Account.GenerateAPIKey.UseCase
{
    [Authorize(RequestSourse.UI)]
    public record GenerateAPIKeyRequest() : IRequest<Result<GenerateAPIKeyResponse>>;
    public record GenerateAPIKeyResponse(string ApiKey);
    public class GenerateAPIKeyInteractor : IRequestHandler<GenerateAPIKeyRequest, Result<GenerateAPIKeyResponse>>
    {
        private readonly IGenerateAPIKeyRepository _generateAPIKeyRepository;
        private readonly ICurrentUserService _currentUserService;
        public GenerateAPIKeyInteractor(IGenerateAPIKeyRepository generateAPIKeyRepository, ICurrentUserService currentUserService)
        {
            _generateAPIKeyRepository = generateAPIKeyRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GenerateAPIKeyResponse>> Handle(GenerateAPIKeyRequest request, CancellationToken cancellationToken)
        {
            var userInfo = _currentUserService.GetCurrentUser();

            var activeAPIKeyExists = await _generateAPIKeyRepository.ActiveAPIKeyExistsAsync(userInfo.UserId, cancellationToken);
            if (activeAPIKeyExists)
                return Result.Fail(GenerateAPIKeyErrors.ActiveAPIKeyAlreadyExists());

            var apiKey = Guid.NewGuid().ToString();

            await _generateAPIKeyRepository.AddAPIKeyAsync(userInfo.UserId, apiKey, cancellationToken);

            return Result.Ok(new GenerateAPIKeyResponse(apiKey));
        }
    }
}
