using Azure.Messaging.ServiceBus;
using Carting.Domain.ExternalServices;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Carting.Infra.ExternalServices
{
    internal class ServiceBusMessageSubscriber : IMessageSubscriber
    {
        private readonly ILogger<ServiceBusMessageSubscriber> _logger;
        private ServiceBusProcessor? _processor;

        public ServiceBusMessageSubscriber(ILogger<ServiceBusMessageSubscriber> logger,  ServiceBusProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        public void SubscribeConsumer<T>(Func<T, bool> messageDelegate)
        {
            if (_processor is null)
                throw new ArgumentNullException(nameof(_processor));

            if (messageDelegate is null)
                throw new ArgumentNullException(nameof(messageDelegate));

            _logger.LogInformation("Subscribing message delegate");
            _processor.ProcessMessageAsync += async (args) =>
            {
                using (_logger.BeginScope(new Dictionary<string, object> {
                    [nameof(args.Message.CorrelationId)] = args.Message.CorrelationId
                }))
                {
                    try
                    {
                        _logger.LogInformation("Received message");

                        var obj = args.Message.Body.ToObjectFromJson<T>(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });

                        var success = messageDelegate.Invoke(obj);

                        if (success)
                        {
                            _logger.LogInformation("Completing message");
                            await args.CompleteMessageAsync(args.Message);
                        }
                        else
                        {
                            _logger.LogInformation("Abandoning message");
                            //negative ack, try to process the message again
                            await args.AbandonMessageAsync(args.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error while processing message");
                        await args.AbandonMessageAsync(args.Message);
                    }
                }
            };

            _processor.ProcessErrorAsync += (args) =>
            {
                _logger.LogError(args.Exception, "Unhandled exception while the message subscriber is running");
                return Task.CompletedTask;
            };

            _processor.StartProcessingAsync().GetAwaiter();
        }
    }
}
