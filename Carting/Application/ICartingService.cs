using Carting.Domain.Entities;

namespace Carting.Application
{
    public interface ICartingService
    {
        List<Item> GetItems(long cartId);
        long AddItem(Item item);
        bool RemoveItem(long itemId);
    }
}
