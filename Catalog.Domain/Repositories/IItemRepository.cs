﻿using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository : IDisposable
    {
        Task<Item?> GetItemAsync(long id);
        Task<List<Item>> GetItemsAsync();
        Task<long> AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task RemoveItemAsync(Item item);
    }
}
