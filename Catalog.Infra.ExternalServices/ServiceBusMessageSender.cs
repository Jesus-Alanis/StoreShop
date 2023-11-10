using Azure.Messaging.ServiceBus;
using Catalog.Domain.ExternalServices;
using System.Text.Json;

namespace Catalog.Infra.ExternalServices
{
    internal class ServiceBusMessageSender : IMessageSender
    {
        private readonly ServiceBusSender _sender;

        public ServiceBusMessageSender(ServiceBusSender serviceBusSender)
        {
            _sender = serviceBusSender;
        }

        public async Task PublishMessageAsJsonAsync(object message)
        {
            if (_sender is null)
                throw new ArgumentNullException(nameof(_sender));

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });
            var serviceBusMessage = new ServiceBusMessage(json) { ContentType = "application/json" };
            await _sender.SendMessageAsync(serviceBusMessage);
        }
    }
}