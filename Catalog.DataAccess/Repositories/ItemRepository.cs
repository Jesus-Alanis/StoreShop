﻿using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private CatalogDbContext? _database;

        public ItemRepository(CatalogDbContext database)
        {
            _database = database;
        }

        public async Task<long> AddItemAsync(Item item)
        {
            if (_database is null)
                return 0;

            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            return item.Id;
        }

        public async Task<Item?> GetItemAsync(long id)
        {
            if (_database is null)
                return null;

            return await _database.Items.FindAsync(id);
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            if (_database is null)
                return Enumerable.Empty<Item>().ToList();

            return await _database.Items.ToListAsync();
        }

        public async Task RemoveItemAsync(Item item)
        {
            if (_database is null)
                return;

            _database.Items.Remove(item);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            if (_database is null)
                return;

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
            if (disposing)
            {
                var db = Interlocked.Exchange(ref _database, null);
                db?.Dispose();
            }
        }
    }
}
