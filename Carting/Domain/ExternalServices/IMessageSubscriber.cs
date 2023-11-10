namespace Carting.Domain.ExternalServices
{
    public interface IMessageSubscriber
    {
        void SubscribeConsumer<T>(Func<T, bool> messageDelegate);
    }
}
