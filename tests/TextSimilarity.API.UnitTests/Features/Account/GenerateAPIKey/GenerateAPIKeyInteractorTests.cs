
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GenerateAPIKey.Repository;
using TextSimilarity.API.Features.Account.GenerateAPIKey.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.GenerateAPIKey
{
    public class GenerateAPIKeyInteractorTests
    {
        private Mock<IGenerateAPIKeyRepository> _generateAPIKeyRepositoryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        [SetUp]
        public void Setup()
        {
            _generateAPIKeyRepositoryMock = new();
            _currentUserServiceMock = new();
        }

        [Test]
        public async Task Handle_Should_ThrowNullReferenceException_WhenUserIsNull()
        {
            //Arrange
            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns((CurrentUserInfo)null);

            var registerRequest = new GenerateAPIKeyRequest();
            var interactor = new GenerateAPIKeyInteractor(_generateAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var act =  async () => await interactor.Handle(registerRequest, default);

            //Assert
            await  act.Should().ThrowAsync<NullReferenceException>();

            _generateAPIKeyRepositoryMock.Verify(
                x => x.AddAPIKeyAsync(
                    It.IsAny<long>(), 
                    It.IsAny<string>(), 
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task Handle_Should_ReturnFailResult_WhenActiveAPIKeyExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _generateAPIKeyRepositoryMock.Setup(
                x => x.ActiveAPIKeyExistsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var registerRequest = new GenerateAPIKeyRequest();
            var interactor = new GenerateAPIKeyInteractor(_generateAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<ActiveAPIKeyAlreadyExistsError>();
        }

        [Test]
        public async Task Handle_Should_ReturnSuccessResult_WhenActiveAPIKeyDoesNotExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _generateAPIKeyRepositoryMock.Setup(
                x => x.ActiveAPIKeyExistsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var registerRequest = new GenerateAPIKeyRequest();
            var interactor = new GenerateAPIKeyInteractor(_generateAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.ApiKey.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Handle_Should_CallAddAPIKeyAsync_WhenActiveAPIKeyDoesNotExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _generateAPIKeyRepositoryMock.Setup(
                x => x.ActiveAPIKeyExistsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var registerRequest = new GenerateAPIKeyRequest();
            var interactor = new GenerateAPIKeyInteractor(_generateAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            _generateAPIKeyRepositoryMock.Verify(
                x => x.AddAPIKeyAsync(
                    It.Is<long>(v => v == user.UserId), 
                    It.IsAny<string>(), 
                    It.IsAny<CancellationToken>()),
                Times.Once );
        }
    }
}
