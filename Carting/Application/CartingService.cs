using Carting.Domain.Entities;
using Carting.Domain.Repositories;

namespace Carting.Application
{
    internal class CartingService : ICartingService
    {
        private readonly ICartRepository _cartRepository;

        public CartingService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<Item> GetItems(long cartId)
        {
            return _cartRepository.GetItems(cartId);
        }

        public long AddItem(Item item)
        {
            if (_cartRepository.Exists(item.Id))
                throw new Exception("Item Duplicated.");

            return _cartRepository.Addtem(item);
        }

        public bool RemoveItem(long itemId)
        {
            if (!_cartRepository.Exists(itemId))
                throw new Exception("Item Not Found.");

            return _cartRepository.RemoveItem(itemId);
        }
    }
}
