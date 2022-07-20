namespace Tjorw.DistributedMessages.ServiceBus;

public class AzureServiceBusProvider<TMessage>
{
    public AzureServiceBusProvider(string connectionString, string queueName)
    {
        ConnectionString = connectionString;
        QueueName = queueName;
    }

    public string ConnectionString { get; }
    public string QueueName { get; }
}
