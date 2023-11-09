using Catalog.Application.Extensions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;

namespace Catalog.Application
{
    internal class CatalogService : ICatalogService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemRepository _itemRepository;

        public CatalogService(ICategoryRepository categoryRepository, IItemRepository itemRepository)
        {
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryAsync(long id)
        {
            return await _categoryRepository.GetCategoryAsync(id);
        }

        public async Task<long> AddCategoryAsync(DTOs.Category categoryDto)
        {
            return await _categoryRepository.AddCategoryAsync(categoryDto.ToEntity());
        }        

        public async Task UpdateCategoryAsync(long categoryId, DTOs.Category categoryDto)
        {
            var category = await GetCategoryAsync(categoryId);
            if (category == null)
                throw new Exception("Category Not Found.");

            categoryDto.MapEntity(category);

            await _categoryRepository.UpdateCategoryAsync(category);

        }

        public async Task RemoveCategoryAndItemsAsync(long categoryId)
        {
            var category = await GetCategoryAsync(categoryId);
            if (category == null)
                throw new Exception("Category Not Found.");
           
            await _categoryRepository.RemoveCategoryAndItemsAsync(category);
        }

        public async Task<IList<Item>> GetPaginatedItemsAsync(long categoryId, int pageSize, int pageIndex)
        {
            return await _itemRepository.GetPaginatedItemsAsync(categoryId, pageSize, pageIndex);
        }

        public async Task<Item> GetItemAsync(long id)
        {
            return await _itemRepository.GetItemAsync(id);
        }

        public async Task<long> AddItemAsync(DTOs.Item itemDto)
        {
            return await _itemRepository.AddItemAsync(itemDto.ToEntity());
        }

        public async Task UpdateItemAsync(long itemId, DTOs.Item itemDto)
        {
            var item = await GetItemAsync(itemId);
            if (item == null)
                throw new Exception("Item Not Found.");

            itemDto.MapEntity(item);

            await _itemRepository.UpdateItemAsync(item);
        }

        public async Task RemoveItemAsync(long itemId)
        {
            var item = await GetItemAsync(itemId);
            if (item == null)
                throw new Exception("Item Not Found.");

            await _itemRepository.RemoveItemAsync(item);
        }
    }
}
