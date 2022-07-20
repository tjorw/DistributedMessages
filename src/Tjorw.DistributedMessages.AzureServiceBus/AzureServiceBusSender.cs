using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace Tjorw.DistributedMessages.ServiceBus;
public class AzureServiceBusSender<TMessage> : IDistributedMessageSender<TMessage>
{
    private readonly string ConnectionString;
    private readonly string QueueName;

    public AzureServiceBusSender(AzureServiceBusProvider<TMessage> provider)
    {
        ConnectionString = provider.ConnectionString;
        QueueName = provider.QueueName;
    }

    public async Task Send(TMessage message)
    {
        var client = new ServiceBusClient(ConnectionString);
        var sender = client.CreateSender(QueueName);
        string payload = "";

        try
        {
            payload = Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(payload);
            await sender.SendMessageAsync(serviceBusMessage);
        }
        catch(Exception ex)
        {
            throw new DistributedMessageSendException($"Failed to send message for type {typeof(TMessage)}", ex, payload);
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }

    public string Serialize(TMessage message)
    {
        return JsonSerializer.Serialize(message);
    }
}
