using Azure.Messaging.ServiceBus;
using Carting.Domain.Entities;
using Carting.Domain.Repositories;
using Carting.Infra.ExternalServices.MessageBroker.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Carting.Infra.ExternalServices.MessageBroker
{
    public class ItemMessageConsumer : BackgroundService
    {
        private readonly IMessageBroker _messagebroker;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private ServiceBusProcessor? _processor;

        public ItemMessageConsumer(IMessageBroker messagebroker, IConfiguration configuration, ICartRepository cartRepository)
        {
            _messagebroker = messagebroker;
            _configuration = configuration;
            _cartRepository = cartRepository;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceBusConfiguration = _configuration.GetSection("AzureServiceBus");
            var topicName = serviceBusConfiguration["TopicName"] ?? string.Empty;
            var subscriptionname = serviceBusConfiguration["SubscriptionName"] ?? string.Empty;

            _processor = _messagebroker.CreateProcessor(topicName, subscriptionname);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_processor is null)
                return;


            _processor.SubscribeConsumer((json) => {
                var success = true;
                
                var dto = JsonSerializer.Deserialize<DTOs.Item>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });

                if (dto is null)
                    return !success;

                _ = _cartRepository.UpdateItems(dto.Id, dto.Name ?? string.Empty, dto.Url ?? string.Empty, dto.Price);

                return success;
            });

            _processor.ProcessErrorAsync += (args) =>
            {               
                //TODO: log errors somewhere

                return Task.CompletedTask;
            };

            await _processor.StartProcessingAsync();

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor is not null)
            {
                await _processor.StopProcessingAsync();
                await _processor.DisposeAsync();
            }


            await base.StopAsync(cancellationToken);
        }
    }
}
