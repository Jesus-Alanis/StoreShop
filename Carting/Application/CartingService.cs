using Carting.Domain.Entities;
using Carting.Domain.Repositories;

namespace Carting.Application
{
    internal class CartingService
    {
        private readonly ICartRepository _cartRepository;

        public CartingService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<Item> GetItems(long cartId)
        {
            return _cartRepository.GetCartItems(cartId);
        }

        public void AddItem(Item item)
        {
            if (_cartRepository.Exists(item.Id))
                return;

            _cartRepository.AddCartItem(item);
        }

        public void RemoveItem(long itemId)
        {
            if (!_cartRepository.Exists(itemId))
                return;

            _cartRepository.RemoveCartItem(itemId);
        }
    }
}
