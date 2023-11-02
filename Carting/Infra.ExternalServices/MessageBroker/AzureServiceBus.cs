using Azure.Messaging.ServiceBus;

namespace Carting.Infra.ExternalServices.MessageBroker
{
    internal class AzureServiceBus : IMessageBroker
    {
        private ServiceBusClient? _client;


        public AzureServiceBus(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
        }

        public ServiceBusProcessor CreateProcessor(string topicName, string subscriptionName)
        {
            if (_client is null)
                throw new ArgumentNullException(nameof(_client));

            return _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
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
                var client = Interlocked.Exchange(ref _client, null);
                if (client is not null)
                    await client.DisposeAsync();
            }
        }
    }
}
