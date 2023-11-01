namespace Catalog.Domain.ExternalServices
{
    public interface IMessageBroker : IDisposable
    {
        Task PublishMessageAsync(object message);
    }
}
