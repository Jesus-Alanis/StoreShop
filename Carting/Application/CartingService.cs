using Carting.Application.Extentions;
using Carting.Domain.Exceptions;
using Carting.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Carting.Application
{
    internal class CartingService : ICartingService
    {
        private readonly ILogger<CartingService> _logger;
        private readonly ICartRepository _cartRepository;

        public CartingService(ILogger<CartingService> logger, ICartRepository cartRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
        }

        public DTOs.Cart GetCart(string cartId)
        {            
            var items = _cartRepository.GetItems(cartId);
            _logger.LogInformation(string.Format("Getting Cart Items: {0} items", items.Count));
            return new DTOs.Cart(cartId, items.Select(i => i.ToDto()));
        }

        public DTOs.Item? GetItem(string cartId, long itemId)
        {
            _logger.LogInformation("Getting Cart Item");
            var item = _cartRepository.GetItem(cartId, itemId);     
            return item?.ToDto();
        }

        public long AddItem(string cartId, DTOs.Item dto)
        {
            _logger.LogInformation("Adding Cart Item");
            var item = dto.ToEntity(cartId);           
            return _cartRepository.Addtem(item);
        }

        public bool RemoveItem(string cartId, long itemId)
        {
            _logger.LogInformation("Getting Cart Item");
            var item = _cartRepository.GetItem(cartId, itemId);
            if (item == null)
                throw new ItemNotFoundException(itemId);

            _logger.LogInformation(string.Format("Deleting Cart Item: {0}", item.Id));
            return _cartRepository.RemoveItem(item.Id);
        }
    }
}
