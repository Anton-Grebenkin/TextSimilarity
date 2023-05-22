using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TextSimilarity.API.Common.Abstractions;
using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Features.Account.GenerateAPIKey.UseCase;
using TextSimilarity.API.Features.Account.GetAPIHistory.UseCase;
using TextSimilarity.API.Features.Account.GetAPIKey.UseCase;
using TextSimilarity.API.Features.Account.Login.UseCase;
using TextSimilarity.API.Features.Account.Register.UseCase;
using TextSimilarity.API.Features.Account.RevokeAPIKey.UseCase;

namespace TextSimilarity.API.Features.Account
{
    public class AccountController : APIControllerBase
    {
        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost(nameof(GenerateAPIKey))]
        public async Task<ActionResult> GenerateAPIKey()
        {
            var result = await Mediator.Send(new GenerateAPIKeyRequest());
            return result.ToActionResult();
        }

        [HttpPost(nameof(RevokeAPIKey))]
        public async Task<ActionResult> RevokeAPIKey()
        {
            var result = await Mediator.Send(new RevokeAPIKeyRequest());
            return result.ToActionResult();
        }

        [HttpGet(nameof(GetAPIKey))]
        public async Task<ActionResult> GetAPIKey()
        {
            var result = await Mediator.Send(new GetAPIKeyRequest());
            return result.ToActionResult();
        }

        [HttpPost(nameof(GetAPIHistory))]
        public async Task<ActionResult> GetAPIHistory(int start, int size, [FromBody] ColumnSort[]? sort)
        {
            var result = await Mediator.Send(new GetAPIHistoryRequest(start, size, sort));
            return result.ToActionResult();
        }
    }
}
