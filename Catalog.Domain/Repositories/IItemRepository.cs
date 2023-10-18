using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item> GetItemAsync(long id);
        Task<List<Item>> GetItemsAsync();
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task RemoveItemAsync(Item item);
    }
}
