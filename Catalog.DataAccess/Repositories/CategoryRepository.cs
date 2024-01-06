using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.DataAccess.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private const string TABLE_NAME = "Category";
        private readonly ILogger<CategoryRepository> _logger;
        private CatalogDbContext? _database;

        public CategoryRepository(ILogger<CategoryRepository> logger, CatalogDbContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<long> AddCategoryAsync(Category category)
        {
            if (_database is null)
                return 0;

            _database.Categories.Add(category);
            _logger.LogInformation(string.Format("Inserting category into {0} table", TABLE_NAME));
            await _database.SaveChangesAsync();

            return category.Id;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            if (_database is null)
                return Enumerable.Empty<Category>().ToList();

            _logger.LogInformation(string.Format("Getting categories from {0} table", TABLE_NAME));
            return await _database.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(long categoryId)
        {
            if (_database is null)
                return null;

            _logger.LogInformation("Finding category.");
            return await _database.Categories.FindAsync(categoryId);
        }

        public async Task RemoveCategoryAndItemsAsync(Category category)
        {
            if (_database is null)
                return;

            //TODO: implement cascade delete EF Core
            _logger.LogInformation("Getting items from category.");
            var items = await _database.Items.Where(item => item.CategoryId == category.Id).ToListAsync();

            _logger.LogInformation("Removing items from category.");
            foreach (var item in items)
                _database.Items.Remove(item);

            _logger.LogInformation("Removing category.");
            _database.Categories.Remove(category);

            await _database.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (_database is null)
                return;

            _logger.LogInformation("Updating category.");
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
