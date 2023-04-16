using FluentResults;

namespace TextSimilarity.API.Features.Account.GenerateAPIKey.UseCase
{
    public static class GenerateAPIKeyErrors
    {
        public static ActiveAPIKeyAlreadyExistsError ActiveAPIKeyAlreadyExists()
        {
            return new ActiveAPIKeyAlreadyExistsError($"Active API key already exists");
        }
    }

    public class ActiveAPIKeyAlreadyExistsError : Error
    {
        public ActiveAPIKeyAlreadyExistsError(string message) : base(message)
        {

        }
    }
}
