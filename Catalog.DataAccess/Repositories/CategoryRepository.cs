using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private CatalogDbContext _database;
        private bool _disposedValue;

        public CategoryRepository(CatalogDbContext database)
        {
            _database = database;
        }

        public async Task<long> AddCategoryAsync(Category category)
        {
            _database.Categories.Add(category);
            await _database.SaveChangesAsync();

            return category.Id;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _database.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(long id)
        {
            return await _database.Categories.FindAsync(id);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _database.Categories.Remove(category);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _database.Categories.Update(category);
            await _database.SaveChangesAsync();
        }

        ~CategoryRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _database.Dispose();
                    _database = null;
                }
                _disposedValue = true;
            }
        }
    }
}
