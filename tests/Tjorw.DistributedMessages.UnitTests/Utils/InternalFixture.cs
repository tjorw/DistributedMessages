using Tjorw.DistributedMessages.Internal;

namespace Tjorw.DistributedMessages.UnitTests.Utils;

public abstract class InternalFixture : IDistributedMessageHandler<DummyMessage>
{

    protected InternalMessageDispatcher<DummyMessage> dispatcher;
    protected List<DummyMessage> handledCommands = new List<DummyMessage>();

    protected InternalFixture()
    {
        dispatcher = new InternalMessageDispatcher<DummyMessage>(this);
    }

    public Task Handle(DummyMessage command)
    {
        handledCommands.Add(command);
        return Task.CompletedTask;
    }


}

