using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private CatalogDbContext? _database;

        public CategoryRepository(CatalogDbContext database)
        {
            _database = database;
        }

        public async Task<long> AddCategoryAsync(Category category)
        {
            if (_database is null)
                return 0;

            _database.Categories.Add(category);
            await _database.SaveChangesAsync();

            return category.Id;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            if (_database is null)
                return Enumerable.Empty<Category>().ToList();

            return await _database.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(long categoryId)
        {
            if (_database is null)
                return null;
            
            return await _database.Categories.FindAsync(categoryId);
        }

        public async Task RemoveCategoryAndItemsAsync(Category category)
        {
            if (_database is null)
                return;

            //TODO: implement cascade delete EF Core
            var items = await _database.Items.Where(item => item.CategoryId == category.Id).ToListAsync();

            foreach (var item in items)
                _database.Items.Remove(item);

            _database.Categories.Remove(category);

            await _database.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (_database is null)
                return;

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
            if (disposing)
            {
                var db = Interlocked.Exchange(ref _database, null);
                db?.Dispose();
            }
        }
    }
}
