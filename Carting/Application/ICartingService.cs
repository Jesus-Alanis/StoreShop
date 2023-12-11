using Carting.Domain.Entities;

namespace Carting.Application
{
    public interface ICartingService
    {
        DTOs.Cart GetCart(string cartId);
        DTOs.Item? GetItem(string cartId, long itemId);
        long AddItem(string cartId, DTOs.Item item);
        bool RemoveItem(string cartId, long itemId);
    }
}
