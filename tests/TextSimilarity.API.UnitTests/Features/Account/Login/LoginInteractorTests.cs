using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.ResultSettings;
using TextSimilarity.API.Common.Security.Authentication;
using TextSimilarity.API.Features.Account.Login.Repository;
using TextSimilarity.API.Features.Account.Login.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.Login
{
    public class LoginInteractorTests
    {
        private Mock<ILoginRepository> _loginRepositoryMock;
        private Mock<IJWTService> _jwtServiceMock;
        private Mock<IPasswordService> _passwordService;

        [SetUp]
        public void Setup()
        {
            _loginRepositoryMock = new();
            _passwordService = new();
            _jwtServiceMock = new();
        }

        [Test]
        public async Task Handle_Should_ReturnFailureResult_WhenUserNotFound()
        {
            //Arrange
            _loginRepositoryMock.Setup(
                x => x.GetUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var loginRequest = new LoginRequest("Login", "Password");
            var interactor = new LoginInteractor(_loginRepositoryMock.Object, _jwtServiceMock.Object, _passwordService.Object);

            //Act
            var result = await interactor.Handle(loginRequest, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<NotFoundError>();
        }

        [Test]
        public async Task Handle_Should_ReturnFailureResult_WhenPasswordIsIncorrect()
        {
            //Arrange
            var loginRequest = new LoginRequest("Login", "Password");

            _loginRepositoryMock.Setup(
                x => x.GetUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Login = loginRequest.Login});

            _passwordService.Setup(
                x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var interactor = new LoginInteractor(_loginRepositoryMock.Object, _jwtServiceMock.Object, _passwordService.Object);

            //Act
            var result = await interactor.Handle(loginRequest, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<UnauthorizedError>();
        }

        [Test]
        public async Task Handle_Should_ReturnSuccessResultWithToken_WhenUserExistsAndPasswordCorrect()
        {
            //Arrange
            var loginRequest = new LoginRequest(It.IsAny<string>(), It.IsAny<string>());
            
            _loginRepositoryMock.Setup(
                x => x.GetUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Login = It.IsAny<string>() });

            _passwordService.Setup(
                x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var token = "Token";
            _jwtServiceMock.Setup(
                x => x.GenerateToken(It.IsAny<long>()))
                .Returns(token);

            var interactor = new LoginInteractor(_loginRepositoryMock.Object, _jwtServiceMock.Object, _passwordService.Object);

            //Act
            var result = await interactor.Handle(loginRequest, default);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.AuthToken.Should().Be(token);
        }
    }
}
