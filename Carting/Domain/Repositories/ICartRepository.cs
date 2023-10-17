using Carting.Domain.Entities;

namespace Carting.Domain.Repositories
{
    public interface ICartRepository : IDisposable
    {
        List<Item> GetCartItems(long cartId);
        void AddCartItem(Item item);
        void RemoveCartItem(long itemId);
        bool Exists(long itemId);

    }
}
