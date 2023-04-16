using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using BCrypt.Net;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Features.Account.Register.Repository;
using TextSimilarity.API.Common.Security.Authentication;

namespace TextSimilarity.API.Features.Account.Register.UseCase
{
    public record RegisterRequest (string Login, string Password) : IRequest<Result>;
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(v => v.Login)
                .NotEmpty().WithMessage($"{nameof(RegisterRequest.Login)} cannot be empty");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage($"{nameof(RegisterRequest.Password)} cannot be empty");
        }
    }
    public class RegisterInteractor : IRequestHandler<RegisterRequest, Result>
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IPasswordService _passwordService;
        public RegisterInteractor(IRegisterRepository registerRepository, IPasswordService passwordService)
        {
            _registerRepository = registerRepository;
            _passwordService = passwordService;
        }
        public async Task<Result> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();

            var userAlreadyExists = await _registerRepository.UserAlreadyExistsAsync(user, cancellationToken);
            if (userAlreadyExists)
                return Result.Fail(RegisterErrors.UserAlreadyExists(user.Login));

            user.PasswordHash = await _passwordService.HashPasswordAsync(request.Password);

            await _registerRepository.AddUserAsync(user, cancellationToken);

            return Result.Ok();
        }
    }
}
