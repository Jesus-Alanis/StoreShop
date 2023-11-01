using Azure.Messaging.ServiceBus;
using Catalog.Domain.ExternalServices;
using System.Text.Json;

namespace Catalog.Infra.ExternalServices
{
    internal class AzureServiceBus : IMessageBroker
    {

        private ServiceBusClient? _client;
        private ServiceBusSender? _sender;


        public AzureServiceBus(string connectionString, string topicName)
        {
            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender(topicName);
        }

        public async Task PublishMessageAsync(object message)
        {
            if (_sender is null)
                throw new ArgumentNullException(nameof(_sender));

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });
            var serviceBusMessage = new ServiceBusMessage(json) { ContentType = "application/json" };
            await _sender.SendMessageAsync(serviceBusMessage);
        }

        ~AzureServiceBus()
        {
            DisposeAsync(false).ConfigureAwait(false).GetAwaiter();
        }

        public void Dispose()
        {
            DisposeAsync(true).ConfigureAwait(false).GetAwaiter();
            GC.SuppressFinalize(this);
        }

        protected async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                var sender = Interlocked.Exchange(ref _sender, null);
                if(sender is not null)
                    await sender.DisposeAsync();

                var client = Interlocked.Exchange(ref _client, null);
                if (client is not null)
                    await client.DisposeAsync();
            }
        }
    }
}
