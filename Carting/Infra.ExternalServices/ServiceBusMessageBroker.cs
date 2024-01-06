using Azure.Messaging.ServiceBus;
using Carting.Domain.ExternalServices;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace Carting.Infra.ExternalServices
{
    internal class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IAzureClientFactory<ServiceBusProcessor> _serviceBusClientFactory;

        public ServiceBusMessageBroker(ILoggerFactory loggerFactory, IAzureClientFactory<ServiceBusProcessor> serviceBusClientFactory)
        {
            _loggerFactory = loggerFactory;
            _serviceBusClientFactory = serviceBusClientFactory;
        }

        public IMessageSubscriber CreateSubscriber(string subscriptionName)
        {
            var processor = _serviceBusClientFactory.CreateClient(subscriptionName);
            return new ServiceBusMessageSubscriber(_loggerFactory.CreateLogger<ServiceBusMessageSubscriber>(), processor);
        }
    }
}
