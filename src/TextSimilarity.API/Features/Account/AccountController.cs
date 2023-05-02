using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TextSimilarity.API.Common.Abstractions;
using TextSimilarity.API.Features.Account.GenerateAPIKey.UseCase;
using TextSimilarity.API.Features.Account.Login.UseCase;
using TextSimilarity.API.Features.Account.Register.UseCase;
using TextSimilarity.API.Features.Account.RevokeAPIKey.UseCase;

namespace TextSimilarity.API.Features.Account
{
    public class AccountController : APIControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("GenerateAPIKey")]
        public async Task<ActionResult> GenerateAPIKey()
        {
            var result = await Mediator.Send(new GenerateAPIKeyRequest());
            return result.ToActionResult();
        }

        [HttpPost("RevokeAPIKey")]
        public async Task<ActionResult> RevokeAPIKey()
        {
            var result = await Mediator.Send(new RevokeAPIKeyRequest());
            return result.ToActionResult();
        }
    }
}
