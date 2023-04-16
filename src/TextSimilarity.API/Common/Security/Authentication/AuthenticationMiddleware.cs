using System.Security.Claims;
using TextSimilarity.API.Common.Security.Authentication;
using TextSimilarity.API.Common.Security.Authorization;

namespace TextSimilarity.API.Common.Security
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJWTService jwtService, IAPIKeyService apiKeyService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token != null)
            {
                if (token.StartsWith("Bearer "))
                {
                    if (jwtService.ValidateToken(token.Substring(7), out var userId) && userId.HasValue)
                    {
                        context.Items[nameof(CurrentUserInfo)] = new CurrentUserInfo { RequestSourse = RequestSourse.UI, UserId = userId.Value };
                    }
                }
                else if (token.StartsWith("ApiKey "))
                {
                    if (apiKeyService.ValidateAPIKey(token.Substring(7), out var userId) && userId.HasValue)
                    {
                        context.Items[nameof(CurrentUserInfo)] = new CurrentUserInfo { RequestSourse = RequestSourse.API, UserId = userId.Value };
                    }
                }

            }

            await _next(context);
        }

    }
}
