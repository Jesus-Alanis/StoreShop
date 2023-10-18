using Carting.Domain.Entities;

namespace Carting.Domain.Repositories
{
    public interface ICartRepository : IDisposable
    {
        List<Item> GetItems(long cartId);
        long Addtem(Item item);
        bool RemoveItem(long itemId);
        bool Exists(long itemId);

    }
}
