namespace Tjorw.DistributedMessages;

public interface IDistributedMessageSender<TMessage>
{
    Task Send(TMessage message);
    string Serialize(TMessage message);
}
