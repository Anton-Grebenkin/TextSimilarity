using TextSimilarity.API.Common.Security;

namespace TextSimilarity.API.Common.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseAuthenticationMiddleware(this WebApplication app)
        {
            app.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
