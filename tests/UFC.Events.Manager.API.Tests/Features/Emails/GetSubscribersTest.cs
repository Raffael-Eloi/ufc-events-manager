using Moq;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Tests.Features.Emails;

internal class GetSubscribersTest
{
    private Mock<ISubscriberRepo> _subscriberRepoMock;
    private IGetSubscribers _getSubscribers;

    [SetUp]
    public void Setup()
    {
        _subscriberRepoMock = new Mock<ISubscriberRepo>();

        _getSubscribers = new GetSubscribers(_subscriberRepoMock.Object);
    }

    [Test]
    public async Task GivenRequest_WhenGetSubscribers_ThenTheSubscribersShouldBeReturned()
    {
        // Arrange

        // Act
        await _getSubscribers.ExecuteAsync();

        // Assert
        _subscriberRepoMock
            .Verify(repo => repo.GetAsync(),
            Times.Once);
    }
}
