using Azure.Messaging.ServiceBus;
using Catalog.Domain.ExternalServices;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Infra.ExternalServices
{
    internal class ServiceBusMessageSender : IMessageSender
    {
        private readonly ILogger<ServiceBusMessageSender> _logger;
        private readonly ServiceBusSender _sender;

        public ServiceBusMessageSender(ILogger<ServiceBusMessageSender> logger, ServiceBusSender serviceBusSender)
        {
            _logger = logger;
            _sender = serviceBusSender;
        }

        public async Task PublishMessageAsJsonAsync(object message)
        {
            if (_sender is null)
                throw new ArgumentNullException(nameof(_sender));

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });
            var serviceBusMessage = new ServiceBusMessage(json) { ContentType = "application/json" };

            _logger.LogInformation(string.Format("Publishing message to Azure Service Bus: {0}", _sender.FullyQualifiedNamespace));
            await _sender.SendMessageAsync(serviceBusMessage);
        }
    }
}