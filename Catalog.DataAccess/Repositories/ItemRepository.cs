using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private bool _disposedValue;
        private CatalogDbContext _database;

        public ItemRepository(CatalogDbContext database)
        {
            _database = database;
        }

        public async Task<long> AddItemAsync(Item item)
        {
            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            return item.Id;
        }

        public async Task<Item> GetItemAsync(long id)
        {
            return await _database.Items.FindAsync(id);
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            return await _database.Items.ToListAsync();
        }

        public async Task RemoveItemAsync(Item item)
        {
            _database.Items.Remove(item);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            _database.Items.Update(item);
            await _database.SaveChangesAsync();
        }

        ~ItemRepository()
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
