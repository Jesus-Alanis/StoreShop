using Catalog.Application;
using DTOs = Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Carting.Tests
{
    public class CatalogServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly ICatalogService _catalogService;

        public CatalogServiceTests(DatabaseFixture fixture)
        {
            _catalogService = new CatalogService(fixture.CategoryRepository, fixture.ItemRepository);
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
