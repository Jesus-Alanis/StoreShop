using Catalog.Application;
using Catalog.Domain.ExternalServices;
using Moq;
using DTOs = Catalog.Application.DTOs;

namespace Carting.Tests
{
    public class CatalogServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly ICatalogService _catalogService;
        private readonly Mock<IMessageBroker> _messageBroker;

        public CatalogServiceTests(DatabaseFixture fixture)
        {
            _messageBroker = new Mock<IMessageBroker>();
            _messageBroker.Setup(s => s.PublishMessageAsync(It.IsAny<object>()));

            _catalogService = new CatalogService(fixture.CategoryRepository, fixture.ItemRepository, _messageBroker.Object);
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
    }
}
