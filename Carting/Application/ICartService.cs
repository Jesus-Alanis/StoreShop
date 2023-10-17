using Carting.Domain.Entities;

namespace Carting.Application
{
    public interface ICartService
    {
        List<Item> GetItems(long cartId);
        void AddItem(Item item);
        void RemoveItem(long itemId);
    }
}
