using FluentResults;
using TextSimilarity.API.Common.ResultSettings;

namespace TextSimilarity.API.Features.Account.Login.UseCase
{
    public static class LoginErrors
    {
        public static NotFoundError UserNotFound(string login)
        {
            return new NotFoundError($"User with login {login} not found");
        }

        public static UnauthorizedError IncorrectPassword()
        {
            return new UnauthorizedError("Password is incorrect");
        }
    }
}
