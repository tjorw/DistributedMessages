namespace Tjorw.DistributedMessages;

public interface IDistributedMessageHandler<TMessage>
{
    Task Handle(TMessage message);
}
