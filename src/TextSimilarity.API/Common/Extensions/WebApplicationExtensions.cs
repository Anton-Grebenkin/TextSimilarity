using TextSimilarity.API.Common.Middleware;
using TextSimilarity.API.Common.Security;

namespace TextSimilarity.API.Common.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseAuthenticationMiddleware(this WebApplication app)
        {
            app.UseMiddleware<AuthenticationMiddleware>();
        }

        public static void UseRequestResponseLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
