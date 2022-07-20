namespace Tjorw.DistributedMessages;

public class DistributedMessageSendException : Exception
{
    public DistributedMessageSendException(string message, Exception innerException, string payload) : base(message, innerException)
    {
        Payload = payload;
    }

    public string Payload { get; }
}
