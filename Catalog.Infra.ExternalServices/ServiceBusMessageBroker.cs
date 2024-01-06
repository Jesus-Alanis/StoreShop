using Azure.Messaging.ServiceBus;
using Catalog.Domain.ExternalServices;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace Catalog.Infra.ExternalServices
{
    internal class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IAzureClientFactory<ServiceBusSender> _serviceBusSenderFactory;

        public ServiceBusMessageBroker(ILoggerFactory loggerFactory, IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory)
        {
            _loggerFactory = loggerFactory;
            _serviceBusSenderFactory = serviceBusSenderFactory;
        }

        public IMessageSender CreateMessageSender(string topicName)
        {
            var serviceBusSender = _serviceBusSenderFactory.CreateClient(topicName);
            return new ServiceBusMessageSender(_loggerFactory.CreateLogger<ServiceBusMessageSender>(), serviceBusSender);
        }
    }
}
