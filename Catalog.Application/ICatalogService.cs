using Catalog.Domain.Entities;

namespace Catalog.Application
{
    public interface ICatalogService
    {
        Task<Category> GetCategoryAsync(long id);
        Task<List<Category>> GetCategoriesAsync();
        Task<long> AddCategoryAsync(DTOs.Category categoryDto);
        Task UpdateCategoryAsync(long categoryId, DTOs.Category categoryDto);
        Task RemoveCategoryAndItemsAsync(long categoryId);

        Task<Item> GetItemAsync(long id);
        Task<IList<Item>> GetPaginatedItemsAsync(long categoryId, int pageSize, int pageIndex);
        Task<long> AddItemAsync(DTOs.Item itemDto);
        Task UpdateItemAsync(long itemId, DTOs.Item itemDto);
        Task RemoveItemAsync(long itemId);
    }
}
