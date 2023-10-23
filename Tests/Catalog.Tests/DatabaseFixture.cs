using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Carting.Tests
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

            CategoryRepository = new CategoryRepository(new CatalogDbContext(contextOptions));
            ItemRepository = new ItemRepository(new CatalogDbContext(contextOptions));
        }

        public void Dispose()
        {
            CategoryRepository.Dispose();
            ItemRepository.Dispose();
        }


    }
}
