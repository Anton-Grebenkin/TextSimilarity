using FluentResults;
using FluentValidation;
using MediatR;
using TextSimilarity.API.Common.Security.Authentication;
using TextSimilarity.API.Features.Account.Login.Repository;

namespace TextSimilarity.API.Features.Account.Login.UseCase
{
    public record LoginRequest(string Login, string Password) : IRequest<Result<LoginResponse>>;
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(v => v.Login)
                .NotEmpty().WithMessage($"{nameof(LoginRequest.Login)} cannot be empty");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage($"{nameof(LoginRequest.Password)} cannot be empty");
        }
    }
    public record LoginResponse(string AuthToken, string Login);
    public class LoginInteractor : IRequestHandler<LoginRequest, Result<LoginResponse>>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJWTService _jwtService;
        public LoginInteractor(ILoginRepository authenticateRepository, IJWTService jwtService, IPasswordService passwordService)
        {
            _loginRepository = authenticateRepository;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _loginRepository.GetUserAsync(request.Login, cancellationToken);
            if (user == null)
                return Result.Fail(LoginErrors.UserNotFound(request.Login));

            var correctPassword = await _passwordService.VerifyPasswordAsync(request.Password, user.PasswordHash);
            if (!correctPassword)
                return Result.Fail(LoginErrors.IncorrectPassword());

            var token = _jwtService.GenerateToken(user.Id);

            return Result.Ok(new LoginResponse(token, user.Login));
        }
    }
}
