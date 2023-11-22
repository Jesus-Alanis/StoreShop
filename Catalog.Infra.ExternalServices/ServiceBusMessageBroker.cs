using Azure.Messaging.ServiceBus;
using Catalog.Domain.ExternalServices;
using Microsoft.Extensions.Azure;

namespace Catalog.Infra.ExternalServices
{
    internal class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly IAzureClientFactory<ServiceBusSender> _serviceBusSenderFactory;

        public ServiceBusMessageBroker(IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory)
        {
            _serviceBusSenderFactory = serviceBusSenderFactory;
        }

        public IMessageSender CreateMessageSender(string topicName)
        {
            var serviceBusSender = _serviceBusSenderFactory.CreateClient(topicName);
            return new ServiceBusMessageSender(serviceBusSender);
        }
    }
}
