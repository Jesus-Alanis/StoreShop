namespace Carting.Domain.ExternalServices
{
    public class MessageBrokerConfiguration
    {
        public string? CartItemsTopic { get; set; }
        public string? CartItemsSubscription { get; set; }
    }
}
