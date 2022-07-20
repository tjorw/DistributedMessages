namespace Tjorw.DistributedMessages;

public interface IDistributedMessageListener<TMessage>
{
    Task HandleException(DistributedMessageHandleException exception);
    TMessage Deserialize(string serializedMessage);
}
