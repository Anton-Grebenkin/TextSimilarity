using TextSimilarity.API.Common.ResultSettings;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.RevokeAPIKey.Repository;
using TextSimilarity.API.Features.Account.RevokeAPIKey.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.RevokeAPIKey
{
    public class RevokeAPIKeyInteractorTests
    {
        private Mock<IRevokeAPIKeyRepository> _revokeAPIKeyRepositoryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        [SetUp]
        public void Setup()
        {
            _revokeAPIKeyRepositoryMock = new();
            _currentUserServiceMock = new();
        }

        [Test]
        public async Task Handle_Should_ThrowNullReferenceException_WhenUserIsNull()
        {
            //Arrange
            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns((CurrentUserInfo)null);

            var registerRequest = new RevokeAPIKeyRequest();
            var interactor = new RevokeAPIKeyInteractor(_revokeAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var act = async () => await interactor.Handle(registerRequest, default);

            //Assert
            await act.Should().ThrowAsync<NullReferenceException>();

            _revokeAPIKeyRepositoryMock.Verify(
                x => x.RevokeActiveAPIKeyAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task Handle_Should_ReturnFailResult_WhenActiveAPIKeyDoesNotExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _revokeAPIKeyRepositoryMock.Setup(
                x => x.ActiveAPIKeyExistsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var registerRequest = new RevokeAPIKeyRequest();
            var interactor = new RevokeAPIKeyInteractor(_revokeAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<NotFoundError>();
        }

        [Test]
        public async Task Handle_Should_ReturnSuccessResult_WhenActiveAPIKeyExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _revokeAPIKeyRepositoryMock.Setup(
                x => x.ActiveAPIKeyExistsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var registerRequest = new RevokeAPIKeyRequest();
            var interactor = new RevokeAPIKeyInteractor(_revokeAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task Handle_Should_CallRevokeAPIKeyAsync_WhenActiveAPIKeyExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _revokeAPIKeyRepositoryMock.Setup(
                 x => x.ActiveAPIKeyExistsAsync(
                     It.IsAny<long>(),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            var registerRequest = new RevokeAPIKeyRequest();
            var interactor = new RevokeAPIKeyInteractor(_revokeAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(registerRequest, default);

            //Assert
            _revokeAPIKeyRepositoryMock.Verify(
                x => x.RevokeActiveAPIKeyAsync(
                    It.Is<long>(v => v == user.UserId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
