using Catalog.Application.Extensions;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.ExternalServices;
using Catalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.Application
{
    internal class CatalogService : ICatalogService
    {
        private readonly ILogger<CatalogService> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMessageSender _messageSender;

        public CatalogService(ILogger<CatalogService> logger, ICategoryRepository categoryRepository, IItemRepository itemRepository, IMessageBroker messageBroker, IOptions<MessageBrokerConfiguration> config)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
            _messageSender = messageBroker.CreateMessageSender(config.Value.CartItemsTopic ?? string.Empty);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<Category?> GetCategoryAsync(long categoryId)
        {
            return await _categoryRepository.GetCategoryAsync(categoryId);
        }

        public async Task<long> AddCategoryAsync(DTOs.Category categoryDto)
        {
            _logger.LogInformation("Creating category");
            return await _categoryRepository.AddCategoryAsync(categoryDto.ToEntity());
        }        

        public async Task UpdateCategoryAsync(long categoryId, DTOs.Category categoryDto)
        {
            _logger.LogInformation("Finding category");
            var category = await GetCategoryAsync(categoryId);
            if (category == null)
                throw new CategoryNotFoundException(categoryId);

            _logger.LogInformation("Mapping category fields.");
            categoryDto.MapEntity(category);

            await _categoryRepository.UpdateCategoryAsync(category);

        }

        public async Task RemoveCategoryAndItemsAsync(long categoryId)
        {
            _logger.LogInformation("Finding category");
            var category = await GetCategoryAsync(categoryId);
            if (category == null)
                throw new CategoryNotFoundException(categoryId);
           
            await _categoryRepository.RemoveCategoryAndItemsAsync(category);
        }

        public async Task<IList<Item>> GetPaginatedItemsAsync(long categoryId, int pageSize, int pageIndex)
        {
            return await _itemRepository.GetPaginatedItemsAsync(categoryId, pageSize, pageIndex);
        }

        public async Task<Item?> GetItemAsync(long itemId)
        {
            return await _itemRepository.GetItemAsync(itemId);
        }

        public async Task<long> AddItemAsync(DTOs.Item itemDto)
        {
            _logger.LogInformation("Creating item");
            return await _itemRepository.AddItemAsync(itemDto.ToEntity());
        }

        public async Task UpdateItemAsync(long itemId, DTOs.Item itemDto, string? correlationId = null)
        {
            _logger.LogInformation("Finding item");
            var item = await GetItemAsync(itemId);
            if (item == null)
                throw new ItemNotFoundException(itemId);

            _logger.LogInformation("Mapping item fields.");
            itemDto.MapEntity(item);

            await _itemRepository.UpdateItemAsync(item);
            await _messageSender.PublishMessageAsJsonAsync(item, correlationId);
        }

        public async Task RemoveItemAsync(long itemId)
        {
            _logger.LogInformation("Finding item");
            var item = await GetItemAsync(itemId);
            if (item == null)
                throw new ItemNotFoundException(itemId);

            await _itemRepository.RemoveItemAsync(item);
        }
    }
}
