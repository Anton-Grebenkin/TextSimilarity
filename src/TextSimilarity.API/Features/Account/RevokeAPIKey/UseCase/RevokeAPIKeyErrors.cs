using TextSimilarity.API.Common.ResultSettings;

namespace TextSimilarity.API.Features.Account.RevokeAPIKey.UseCase
{
    public static class RevokeAPIKeyErrors
    {
        public static NotFoundError ActiveAPIKeyNotFound()
        {
            return new NotFoundError("Active API key not found");
        }
    }
}
