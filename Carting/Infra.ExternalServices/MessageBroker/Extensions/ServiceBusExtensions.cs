using Azure.Messaging.ServiceBus;

namespace Carting.Infra.ExternalServices.MessageBroker.Extensions
{
    internal static class ServiceBusExtensions
    {

        internal static void SubscribeConsumer(this ServiceBusProcessor processor, Func<string, bool> messageDelegate)
        {
            if (processor is null) 
                throw new ArgumentNullException(nameof(processor));

            if (messageDelegate is null)
                throw new ArgumentNullException(nameof(messageDelegate));

            processor.ProcessMessageAsync += async (args) =>
            {
                try
                {
                    string body = args.Message.Body.ToString();

                    var success = messageDelegate.Invoke(body);

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
        }
    }
}
