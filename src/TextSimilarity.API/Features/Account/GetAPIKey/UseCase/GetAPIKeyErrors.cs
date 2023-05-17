using TextSimilarity.API.Common.ResultSettings;

namespace TextSimilarity.API.Features.Account.GetAPIKey.UseCase
{
    public static class GetAPIKeyErrors
    {
        public static NotFoundError APIKeyNotFound()
        {
            return new NotFoundError($"API key not found");
        }
    }
}
