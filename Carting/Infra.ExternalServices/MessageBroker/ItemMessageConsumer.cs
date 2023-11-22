using Carting.Domain.ExternalServices;
using Carting.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Carting.Infra.ExternalServices.MessageBroker
{
    public class ItemMessageConsumer : BackgroundService
    {
        private const string TOPIC_SUBSCRIPTION_NAME_SETTING = "CartItemsSubscription";

        private readonly IMessageBroker _messagebroker;
        private readonly MessageBrokerConfiguration _messageBrokerConfiguration;
        private readonly ICartRepository _cartRepository;
        private IMessageSubscriber? _processor;

        public ItemMessageConsumer(IMessageBroker messagebroker, ICartRepository cartRepository, IOptions<MessageBrokerConfiguration> config)
        {
            _messagebroker = messagebroker;
            _messageBrokerConfiguration = config.Value;
            _cartRepository = cartRepository;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _processor = _messagebroker.CreateSubscriber(_messageBrokerConfiguration.CartItemsSubscription ?? string.Empty);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_processor is null)
                return;

            _processor.SubscribeConsumer<DTOs.Item>((dto) => {
                
                if (dto is null)
                    return false;

                _ = _cartRepository.UpdateItems(dto.Id, dto.Name ?? string.Empty, dto.Url ?? string.Empty, dto.Price);

                return true;
            });           

            await Task.CompletedTask;
        }
    }
}
