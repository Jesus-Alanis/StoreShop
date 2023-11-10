using Azure.Messaging.ServiceBus;
using Carting.Domain.ExternalServices;
using Microsoft.Extensions.Azure;

namespace Carting.Infra.ExternalServices
{
    internal class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly IAzureClientFactory<ServiceBusProcessor> _serviceBusClientFactory;

        public ServiceBusMessageBroker(IAzureClientFactory<ServiceBusProcessor> serviceBusClientFactory)
        {
            _serviceBusClientFactory = serviceBusClientFactory;
        }

        public IMessageSubscriber CreateSubscriber(string subscriptionName)
        {
            var processor = _serviceBusClientFactory.CreateClient(subscriptionName);
            return new ServiceBusMessageSubscriber(processor);
        }
    }
}
