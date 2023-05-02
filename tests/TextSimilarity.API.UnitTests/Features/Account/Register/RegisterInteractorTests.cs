using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.Security.Authentication;
using TextSimilarity.API.Features.Account.Register.Repository;
using TextSimilarity.API.Features.Account.Register.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.Register
{
    public class RegisterInteractorTests
    {
        private Mock<IRegisterRepository> _registerRepositoryMock;
        private Mock<IPasswordService> _passwordService;
        private Mock<IJWTService> _jwtServiceMock;
        private RegisterInteractor _interactor;

        [SetUp]
        public void Setup()
        {
            _registerRepositoryMock = new();
            _passwordService = new();
            _jwtServiceMock = new();

            _interactor = new RegisterInteractor(_registerRepositoryMock.Object, _passwordService.Object, _jwtServiceMock.Object);
        }

        [Test]
        public async Task Handle_Should_ReturnFailureResult_WhenUserAlreadyExists()
        {
            //Arrange
            _registerRepositoryMock.Setup(
                x => x.UserAlreadyExistsAsync(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var registerRequest = new RegisterRequest("Login", "Password");

            //Act
            var result = await _interactor.Handle(registerRequest, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<UserAlreadyExistsError>();
        }

        [Test]
        public async Task Handle_Should_ReturnSuccessResultWithToken_WhenUserDoesNotExists()
        {
            //Arrange
            _registerRepositoryMock.Setup(
                x => x.UserAlreadyExistsAsync(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var token = "Token";
            _jwtServiceMock.Setup(
                x => x.GenerateToken(It.IsAny<long>()))
                .Returns(token);

            var registerRequest = new RegisterRequest("Login", "Password");

            //Act
            var result = await _interactor.Handle(registerRequest, default);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.AuthToken.Should().Be(token);
        }

        [Test]
        public async Task Handle_Should_CallAddUserAsync_WhenUserDoesNotExists()
        {
            //Arrange
            var registerRequest = new RegisterRequest("Login", "Password");
            var passwordHash = "Hash";

            _registerRepositoryMock.Setup(
                x => x.UserAlreadyExistsAsync(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordService.Setup(
                x => x.HashPasswordAsync(registerRequest.Password))
                .ReturnsAsync(passwordHash);

            //Act
            var result = await _interactor.Handle(registerRequest, default);

            //Assert
            _registerRepositoryMock.Verify(
                x => x.AddUserAsync(
                    It.Is<User>(u => u.Login == registerRequest.Login && u.PasswordHash == passwordHash), 
                    It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Test]
        public async Task Handle_Should_CallHashPasswordAsync_WhenUserDoesNotExists()
        {
            //Arrange
            _registerRepositoryMock.Setup(
                x => x.UserAlreadyExistsAsync(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var registerRequest = new RegisterRequest("Login", "Password");

            //Act
            var result = await _interactor.Handle(registerRequest, default);

            //Assert
            _passwordService.Verify(
                x => x.HashPasswordAsync(It.Is<string>(s => s == registerRequest.Password)),
                Times.Once
                );
        }
    }
}
