namespace Catalog.Domain.ExternalServices
{
    public interface IMessageSender
    {
        Task PublishMessageAsJsonAsync(object message, string? correlationId = null);
    }
}
