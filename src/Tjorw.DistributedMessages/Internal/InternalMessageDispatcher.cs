using System.Text.Json;
namespace Tjorw.DistributedMessages.Internal;

public class InternalMessageDispatcher<TMessage> : IDistributedMessageListener<TMessage>, IDistributedMessageSender<TMessage>
{
    private readonly IDistributedMessageHandler<TMessage> MessageHandler;

    public InternalMessageDispatcher(IDistributedMessageHandler<TMessage> messageHandler)
    {
        MessageHandler = messageHandler;
    }

    public virtual async Task Send(TMessage message)
    {
        var serializedMessage = send(message);
        await receive(serializedMessage);
    }

    public virtual Task HandleException(DistributedMessageHandleException exception)
    {
        return Task.CompletedTask;
    }

    public virtual string Serialize(TMessage message)
    {
        return JsonSerializer.Serialize(message);
    }

    public TMessage Deserialize(string serializedMessage)
    {
        var message = JsonSerializer.Deserialize<TMessage>(serializedMessage);
        return message!;
    }

    private string send(TMessage message)
    {
        var serializedMessage = "";

        try
        {
            serializedMessage = Serialize(message);

        }
        catch (Exception ex)
        {
            throw new DistributedMessageSendException($"Failed to send message of type {typeof(TMessage)}", ex, serializedMessage);
        }

        return serializedMessage;
    }

    private async Task receive(string serializedMessage)
    {
        try
        {
            var payload = Deserialize(serializedMessage);
            await MessageHandler.Handle(payload);
        }
        catch (Exception ex)
        {
            await HandleException(new DistributedMessageHandleException($"Failed to handle message of type {typeof(TMessage)}", ex, serializedMessage));
        }
    }

}
