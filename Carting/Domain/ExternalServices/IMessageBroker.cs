namespace Carting.Domain.ExternalServices
{
    public interface IMessageBroker
    {
        IMessageSubscriber CreateSubscriber(string subscriptionName);
    }
}
