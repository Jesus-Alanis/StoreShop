using Carting.DataAccess.Repositories;
using Carting.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Carting.Tests
{
    public  class DatabaseFixture : IDisposable
    {

        public ICartRepository CartRepository { get; private set; }

        public DatabaseFixture()
        {
            var loggerFactory = new NullLoggerFactory();
            CartRepository = new CartRepository(loggerFactory.CreateLogger<CartRepository>(), ":memory:");
        }

        public void Dispose()
        {
            CartRepository.Dispose();
        }

       
    }
}
