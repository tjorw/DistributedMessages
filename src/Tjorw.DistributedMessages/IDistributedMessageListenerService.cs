namespace Tjorw.DistributedMessages;

public interface IDistributedMessageListenerService
{
    Task Start();
    Task Stop();
}
