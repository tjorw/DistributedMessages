namespace Tjorw.DistributedMessages.UnitTests.Utils;

public class DummyMessage
{
    public DummyMessage(string data)
    {
        Data = data;
    }

    public string Data { get; }
}

