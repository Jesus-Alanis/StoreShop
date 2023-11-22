namespace Catalog.Domain.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(long itemId) : base($"Item [{itemId}] Not Found")
        {            
        }
       
    }
}
