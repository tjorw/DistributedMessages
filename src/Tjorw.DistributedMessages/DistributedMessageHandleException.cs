namespace Tjorw.DistributedMessages;

public class DistributedMessageHandleException : Exception
{
    public DistributedMessageHandleException(string message, Exception innerException, string payload) : base(message, innerException)
    {
        Payload = payload;
    }

    public string Payload { get; }
}
