using Moq;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;
using UFC.Events.Manager.API.Repositories;

namespace UFC.Events.Manager.API.Tests.Features.UFCEvents;

internal class GetUfcEventsTest
{
    private Mock<IUfcEventRepo> _ufcEventRepoMock;
    private IGetUfcEvents _getUfcEvents;

    [SetUp]
    public void Setup()
    {
        _ufcEventRepoMock = new Mock<IUfcEventRepo>();

        _getUfcEvents = new GetUfcEvents(_ufcEventRepoMock.Object);
    }
    
    [Test]
    public async Task GivenRequest_WhenGetEvents_ThenTheEventsShouldBeReturned()
    {
        // Arrange

        // Act
        await _getUfcEvents.ExecuteAsync();

        // Assert
        _ufcEventRepoMock
            .Verify(repo => repo.GetAsync(), 
            Times.Once);
    }
}