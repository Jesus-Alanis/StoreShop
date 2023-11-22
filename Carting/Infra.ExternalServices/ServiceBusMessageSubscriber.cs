using Azure.Messaging.ServiceBus;
using Carting.Domain.ExternalServices;
using System.Text.Json;

namespace Carting.Infra.ExternalServices
{
    internal class ServiceBusMessageSubscriber : IMessageSubscriber
    {
        private ServiceBusProcessor? _processor;

        public ServiceBusMessageSubscriber(ServiceBusProcessor processor)
        {
            _processor = processor;
        }

        public void SubscribeConsumer<T>(Func<T, bool> messageDelegate)
        {
            if (_processor is null)
                throw new ArgumentNullException(nameof(_processor));

            if (messageDelegate is null)
                throw new ArgumentNullException(nameof(messageDelegate));

            _processor.ProcessMessageAsync += async (args) =>
            {
                try
                {
                    var obj = args.Message.Body.ToObjectFromJson<T>(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true });

                    var success = messageDelegate.Invoke(obj);

                    if (success)
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                    else
                    {
                        //TODO: log errors somewhere
                        //negative ack, try to process the message again
                        await args.AbandonMessageAsync(args.Message);
                    }
                }
                catch (Exception ex)
                {
                    //TODO: log errors somewhere
                    await args.AbandonMessageAsync(args.Message);
                }
            };

            _processor.ProcessErrorAsync += (args) =>
            {
                //TODO: log errors somewhere

                return Task.CompletedTask;
            };

            _processor.StartProcessingAsync().GetAwaiter();
        }
    }
}
