using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Common.ResultSettings;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.GetAPIKey.Repository;
using TextSimilarity.API.Features.Account.GetAPIKey.UseCase;

namespace TextSimilarity.API.UnitTests.Features.Account.GetAPIKey
{
    public class GetAPIKeyInteractorTests
    {
        private Mock<IGetAPIKeyRepository> _getAPIKeyRepositoryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        [SetUp]
        public void Setup()
        {
            _getAPIKeyRepositoryMock = new();
            _currentUserServiceMock = new();
        }

        [Test]
        public async Task Handle_Should_ThrowNullReferenceException_WhenUserIsNull()
        {
            //Arrange
            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns((CurrentUserInfo)null);

            var request = new GetAPIKeyRequest();
            var interactor = new GetAPIKeyInteractor(_getAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var act = async () => await interactor.Handle(request, default);

            //Assert
            await act.Should().ThrowAsync<NullReferenceException>();

            _getAPIKeyRepositoryMock.Verify(
                x => x.GetAPIKeyAsync(
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

            _getAPIKeyRepositoryMock.Setup(
                x => x.GetAPIKeyAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((APIKey)null);

            var request = new GetAPIKeyRequest();
            var interactor = new GetAPIKeyInteractor(_getAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(request, default);

            //Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().BeOfType<NotFoundError>();
        }

        [Test]
        public async Task Handle_Should_ReturnSuccessResult_WhenActiveAPIKeySSExists()
        {
            //Arrange
            var user = new CurrentUserInfo { UserId = 1 };
            var apiKey = new APIKey { Value = Guid.NewGuid().ToString() };

            _currentUserServiceMock.Setup(
                x => x.GetCurrentUser())
                .Returns(user);

            _getAPIKeyRepositoryMock.Setup(
                x => x.GetAPIKeyAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiKey);

            var request = new GetAPIKeyRequest();
            var interactor = new GetAPIKeyInteractor(_getAPIKeyRepositoryMock.Object, _currentUserServiceMock.Object);

            //Act
            var result = await interactor.Handle(request, default);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.ApiKey.Should().NotBeNullOrEmpty();
            result.Value.ApiKey.Should().Be(apiKey.Value);
        }
    }
}
