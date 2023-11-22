using Carting.Domain.Entities;

namespace Carting.Domain.Repositories
{
    public interface ICartRepository : IDisposable
    {
        List<Item> GetItems(string cartId);
        Item GetItem(string cartId, long itemId);
        long Addtem(Item item);
        int UpdateItems(long itemId, string name, string url, double price);
        bool RemoveItem(long itemId);
        bool Exists(string cartId, long itemId);

    }
}
