﻿using Catalog.Domain.Entities;

namespace Catalog.Application
{
    public interface ICatalogService
    {
        Task<Category> GetCategoryAsync(long id);
        Task<List<Category>> GetCategoriesAsync();
        Task AddCategoryAsync(DTOs.Category categoryDto);
        Task UpdateCategoryAsync(long categoryId, DTOs.Category categoryDto);
        Task RemoveCategoryAsync(long categoryId);

        Task<Item> GetItemAsync(long id);
        Task<List<Item>> GetItemsAsync();
        Task AddItemAsync(DTOs.Item itemDto);
        Task UpdateItemAsync(long itemId, DTOs.Item itemDto);
        Task RemoveItemAsync(long itemId);
    }
}