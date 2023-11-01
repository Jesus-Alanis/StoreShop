using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface ICategoryRepository : IDisposable
    {
        Task<Category?> GetCategoryAsync(long id);
        Task<List<Category>> GetCategoriesAsync();
        Task<long> AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAndItemsAsync(Category category);
    }
}
