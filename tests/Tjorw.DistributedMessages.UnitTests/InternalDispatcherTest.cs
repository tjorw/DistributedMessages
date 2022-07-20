using Tjorw.DistributedMessages.UnitTests.Utils;

namespace Tjorw.DistributedMessages.UnitTests;

public class InternalDispatcherTest : InternalFixture
{
    [Fact]
    public async Task ShouldCallHandlerExactlyOnce()
    {
        //Arrange
        var sut = dispatcher;

        //Act
        await sut.Send(new DummyMessage("Test"));

        //Assert
        Assert.Single(handledCommands);
    }
}
