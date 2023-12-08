using Catalog.Application;
using Catalog.Domain.ExternalServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using DTOs = Catalog.Application.DTOs;

namespace Catalog.Tests
{
    public class CatalogServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly ICatalogService _catalogService;
        private readonly Mock<IMessageBroker> _messageBroker;
        private readonly Mock<IMessageSender> _messageSender;
        private readonly MessageBrokerConfiguration _brokerConfig;

        public CatalogServiceTests(DatabaseFixture fixture)
        {
            _brokerConfig = new MessageBrokerConfiguration { CartItemsTopic = "cart_items" };

            _messageSender = new Mock<IMessageSender>();
            _messageSender.Setup(sender => sender.PublishMessageAsJsonAsync(It.IsAny<object>()));

            _messageBroker = new Mock<IMessageBroker>();
            _messageBroker.Setup(s => s.CreateMessageSender(_brokerConfig.CartItemsTopic)).Returns(_messageSender.Object);

            
            var config = new Mock<IOptions<MessageBrokerConfiguration>>();
            config.Setup(s => s.Value).Returns(_brokerConfig);

            var loggerFactory = new NullLoggerFactory();
            _catalogService = new CatalogService(loggerFactory.CreateLogger<CatalogService>(), fixture.CategoryRepository, fixture.ItemRepository, _messageBroker.Object, config.Object);
        }

        [Fact]
        public async void AddCategoryAsync_PassCategory_ShouldSaveCategory()
        {
            var dto = new DTOs.Category
            {
                Name = "Bread",
                Url = "https://some.domain.com"
            };

            var id = await _catalogService.AddCategoryAsync(dto);

            Assert.True(id > 0);
        }

        [Fact]
        public async void GetCategoryAsync_PassId_ShouldGetCategory()
        {
            var dto = new DTOs.Category
            {
                Name = "Bread",
                Url = "https://some.domain.com"
            };

            var id = await _catalogService.AddCategoryAsync(dto);
            var category = await _catalogService.GetCategoryAsync(id);

            Assert.Equal(id, category.Id);
        }

        [Fact]
        public async void RemoveCategoryAsync_PassId_ShouldRemoveCategory()
        {
            var dto = new DTOs.Category
            {
                Name = "Bread",
                Url = "https://some.domain.com"
            };

            var id = await _catalogService.AddCategoryAsync(dto);
            await _catalogService.RemoveCategoryAndItemsAsync(id);
            var category = await _catalogService.GetCategoryAsync(id);

            Assert.Null(category);
        }

        [Fact]
        public async void UpdateItemAsync_PassItem_ShouldSendMessage()
        {
            var item = new DTOs.Item
            {
                Name= "lightbulb", 
                CategoryId= 1, 
                Price= 3.25, 
                Amount= 3
            };

            var itemId = await _catalogService.AddItemAsync(item);
            item.Name = "Updated Name";

            await _catalogService.UpdateItemAsync(itemId, item);

            _messageSender.Verify(sender => sender.PublishMessageAsJsonAsync(It.IsAny<object>()), Times.Once());
        }
    }
}
