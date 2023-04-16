using TextSimilarity.API.Features.Account.Login.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.Login
{
    public  class LoginRequestValidatorTests
    {
        private LoginRequestValidator _registerRequestValidator;

        [SetUp]
        public void Setup()
        {
            _registerRequestValidator = new LoginRequestValidator();
        }

        [Test]
        public void Validator_Should_HasErrorForLogin_WhenLoginIsNull()
        {
            //Arrange
            var request = new LoginRequest(null, "Password");

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Login)
                .WithErrorMessage($"{nameof(LoginRequest.Login)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForLogin_WhenLoginIsEmpty()
        {
            //Arrange
            var request = new LoginRequest(string.Empty, "Password");

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Login)
                .WithErrorMessage($"{nameof(LoginRequest.Login)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForPassword_WhenPasswordIsNull()
        {
            //Arrange
            var request = new LoginRequest("Login", null);

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage($"{nameof(LoginRequest.Password)} cannot be empty");
        }

        [Test]
        public void Validator_Should_HasErrorForPassword_WhenPasswordIsEmpty()
        {
            //Arrange
            var request = new LoginRequest("Login", string.Empty);

            //Act
            var result = _registerRequestValidator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage($"{nameof(LoginRequest.Password)} cannot be empty");
        }
    }
}
