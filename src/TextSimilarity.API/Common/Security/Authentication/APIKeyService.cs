namespace TextSimilarity.API.Common.Security.Authentication
{
    public interface IAPIKeyService
    {
        bool ValidateAPIKey(string apiKey, out long? userId);
    }
    public class APIKeyService : IAPIKeyService
    {
        private readonly IAPIKeyRepository _apiKeyRepository;
        public APIKeyService(IAPIKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }
        public bool ValidateAPIKey(string apiKey, out long? userId)
        {
            userId = null;
            var key = _apiKeyRepository.GetAPIKeyAsync(apiKey);

            if (key == null || key.IsActive == false)
                return false;

            userId = key.UserId;
            return true;
        }
    }
}
