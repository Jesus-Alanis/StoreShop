namespace Catalog.Domain.ExternalServices
{
    public interface IMessageBroker 
    {
        IMessageSender CreateMessageSender(string topicName);
    }
}
