using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Catalog.Tests
{
    public class DatabaseFixture : IDisposable
    {

        public ICategoryRepository CategoryRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }

        public DatabaseFixture()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase("DatabaseTests")
                .Options;

            var loggerFactory = new NullLoggerFactory();
            CategoryRepository = new CategoryRepository(loggerFactory.CreateLogger<CategoryRepository>(), new CatalogDbContext(contextOptions));
            ItemRepository = new ItemRepository(loggerFactory.CreateLogger<ItemRepository>(), new CatalogDbContext(contextOptions));
        }

        public void Dispose()
        {
            CategoryRepository.Dispose();
            ItemRepository.Dispose();
        }


    }
}
