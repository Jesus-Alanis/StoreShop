using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository : IDisposable
    {
        Task<Item?> GetItemAsync(long id);
        Task<IList<Item>> GetPaginatedItemsAsync(long categoryId, int pageSize, int pageIndex = 1);
        Task<long> AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task RemoveItemAsync(Item item);
    }
}
