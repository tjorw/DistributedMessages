using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace Tjorw.DistributedMessages.ServiceBus;

public class AzureServiceBusListener<TMessage> : IDistributedMessageListenerService, IDistributedMessageListener<TMessage>, IAsyncDisposable
{
    private readonly IDistributedMessageHandler<TMessage> MessageHandler;
    private readonly string ConnectionString;
    private readonly string QueueName;
    private ServiceBusClient Client;
    private ServiceBusProcessor Processor;

    public AzureServiceBusListener(AzureServiceBusProvider<TMessage> provider, IDistributedMessageHandler<TMessage> messageHandler)
    {
        MessageHandler = messageHandler;
        ConnectionString = provider.ConnectionString;
        QueueName = provider.QueueName;
        Client = new ServiceBusClient(ConnectionString);

        Processor = Client.CreateProcessor(QueueName);
        Processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
        Processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
    }

    public async Task Start()
    {
        if (!Processor.IsProcessing)
        {
            await Processor.StartProcessingAsync();
        }
    }

    private async Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        try
        {
            await HandleException(new DistributedMessageHandleException($"Failed to handle message of type {typeof(TMessage)}", arg.Exception, ""));
        }
        catch { }
    }

    private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        var payload = arg.Message.Body.ToString();

        try
        {
            var message = Deserialize(payload);
            await MessageHandler.Handle(message!);
        }
        catch (Exception ex)
        {
            await HandleException(new DistributedMessageHandleException($"Failed to handle message of type {typeof(TMessage)}", ex, payload));
        }

    }

    public async Task Stop()
    {
        if (Processor!.IsProcessing)
        {
            await Processor.StopProcessingAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Processor!.IsProcessing)
        {
            await Processor!.StopProcessingAsync();
        }

        await Processor!.DisposeAsync();
        await Client!.DisposeAsync();
    }

    public virtual Task HandleException(DistributedMessageHandleException exception)
    {
        return Task.CompletedTask;
    }

    public TMessage Deserialize(string serializedMessage)
    {
        var message = JsonSerializer.Deserialize<TMessage>(serializedMessage);
        return message!;
    }
}
