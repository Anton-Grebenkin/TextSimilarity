using FluentResults;

namespace TextSimilarity.API.Features.Account.Register.UseCase
{
    public static class RegisterErrors
    {
        public static UserAlreadyExistsError UserAlreadyExists(string login)
        {
            return new UserAlreadyExistsError(login);
        }
    }

    public class UserAlreadyExistsError : Error
    {
        public UserAlreadyExistsError(string login) : base($"User with login {login} already exists") {}
    }
}
