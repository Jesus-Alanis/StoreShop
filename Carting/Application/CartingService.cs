using Carting.Application.Extentions;
using Carting.Domain.Exceptions;
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

        public DTOs.Cart GetCart(string cartId)
        {
            var items = _cartRepository.GetItems(cartId);
            return new DTOs.Cart(cartId, items.Select(i => i.ToDto()));
        }

        public DTOs.Item GetItem(string cartId, long itemId)
        {
            var item = _cartRepository.GetItem(cartId, itemId);
            return item.ToDto();
        }

        public long AddItem(string cartId, DTOs.Item dto)
        {
            var item = dto.ToEntity(cartId);
            return _cartRepository.Addtem(item);
        }

        public bool RemoveItem(string cartId, long itemId)
        {
            if (!_cartRepository.Exists(cartId, itemId))
                throw new ItemNotFoundException(itemId);

            return _cartRepository.RemoveItem(itemId);
        }
    }
}
