using Moq;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.Emails.AddSubscribers;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Tests.Features.Emails;

internal class AddSubscribersTest
{
    private Mock<ISubscriberRepo> _subscriberRepoMock;
    private IAddSubscribers _addSubscribers;

    [SetUp]
    public void Setup()
    {
        _subscriberRepoMock = new Mock<ISubscriberRepo>();

        _addSubscribers = new AddSubscribers(_subscriberRepoMock.Object);
    }

    [Test]
    public async Task GivenNewEmail_WhenAddingSubscriber_ThenTheSubscriberShouldBePersisted()
    {
        // Arrange
        string email = "fan@example.com";

        _subscriberRepoMock
            .Setup(repo => repo.ExistsAsync(email))
            .ReturnsAsync(false);

        // Act
        await _addSubscribers.ExecuteAsync(email);

        // Assert
        _subscriberRepoMock
            .Verify(repo => repo.AddAsync(It.Is<Subscriber>(s => s.Email == email)),
            Times.Once);
    }

    [Test]
    public async Task GivenDuplicateEmail_WhenAddingSubscriber_ThenTheSubscriberShouldNotBePersisted()
    {
        // Arrange
        string email = "fan@example.com";

        _subscriberRepoMock
            .Setup(repo => repo.ExistsAsync(email))
            .ReturnsAsync(true);

        // Act
        await _addSubscribers.ExecuteAsync(email);

        // Assert
        _subscriberRepoMock
            .Verify(repo => repo.AddAsync(It.IsAny<Subscriber>()),
            Times.Never);
    }
}
