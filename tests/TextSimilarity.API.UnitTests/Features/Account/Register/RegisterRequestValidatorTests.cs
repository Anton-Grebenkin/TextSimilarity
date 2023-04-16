using TextSimilarity.API.Features.Account.Register.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.Register
{
    public class RegisterRequestValidatorTests
    {
        private RegisterRequestValidator _registerRequestValidator;

        [Test]
        public void Validator_Should_HasErrorForLogin_WhenLoginIsNull()
        {
            //Arrange
            _registerRequestValidator = new RegisterRequestValidator();
            var request = new RegisterRequest(null, "Password");

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Login)
                .WithErrorMessage($"{nameof(RegisterRequest.Login)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForLogin_WhenLoginIsEmpty()
        {
            //Arrange
            _registerRequestValidator = new RegisterRequestValidator();
            var request = new RegisterRequest(string.Empty, "Password");

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Login)
                .WithErrorMessage($"{nameof(RegisterRequest.Login)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForPassword_WhenPasswordIsNull()
        {
            //Arrange
            _registerRequestValidator = new RegisterRequestValidator();
            var request = new RegisterRequest("Login", null);

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage($"{nameof(RegisterRequest.Password)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForPassword_WhenPasswordIsEmpty()
        {
            //Arrange
            _registerRequestValidator = new RegisterRequestValidator();
            var request = new RegisterRequest("Login", string.Empty);

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage($"{nameof(RegisterRequest.Password)} cannot be empty");
        }
    }
}
