using Azure.Messaging.ServiceBus;

namespace Carting.Infra.ExternalServices.MessageBroker
{
    public interface IMessageBroker : IDisposable
    {
        ServiceBusProcessor CreateProcessor(string topicName, string subscriptionName);
    }
}
