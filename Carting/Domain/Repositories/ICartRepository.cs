using Carting.Domain.Entities;

namespace Carting.Domain.Repositories
{
    public interface ICartRepository : IDisposable
    {
        List<Item> GetItems(string cartId);
        Item GetItem(string cartId, long itemId);
        long Addtem(Item item);
        bool RemoveItem(long itemId);
        bool Exists(string cartId, long itemId);

    }
}
