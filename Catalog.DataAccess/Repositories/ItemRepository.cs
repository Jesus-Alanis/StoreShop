﻿using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.DataAccess.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private const string TABLE_NAME = "Item";

        private readonly ILogger<ItemRepository> _logger;
        private CatalogDbContext? _database;

        public ItemRepository(ILogger<ItemRepository> logger, CatalogDbContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<long> AddItemAsync(Item item)
        {
            if (_database is null)
                return 0;

            _database.Items.Add(item);
            _logger.LogInformation(string.Format("Inserting item into {0} table", TABLE_NAME));
            await _database.SaveChangesAsync();

            return item.Id;
        }

        public async Task<Item?> GetItemAsync(long itemId)
        {
            if (_database is null)
                return null;

            _logger.LogInformation("Finding item.");
            return await _database.Items.FindAsync(itemId);
        }

        public async Task<IList<Item>> GetPaginatedItemsAsync(long categoryId, int pageSize, int pageIndex = 1)
        {
            if (_database is null)
                return Array.Empty<Item>();

            _logger.LogInformation(string.Format("Getting items from {0} table", TABLE_NAME));
            return await _database.Items.Where(item => item.CategoryId == categoryId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task RemoveItemAsync(Item item)
        {
            if (_database is null)
                return;

            _database.Items.Remove(item);
            _logger.LogInformation("Removing item.");
            await _database.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            if (_database is null)
                return;

            _database.Items.Update(item);
            _logger.LogInformation("Updating item.");
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
            if (disposing)
            {
                var db = Interlocked.Exchange(ref _database, null);
                db?.Dispose();
            }
        }
    }
}
