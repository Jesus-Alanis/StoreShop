using Carting.DataAccess.Repositories;
using Carting.Domain.Repositories;

namespace Carting.Tests
{
    public  class DatabaseFixture : IDisposable
    {

        public ICartRepository CartRepository { get; private set; }

        public DatabaseFixture()
        {
            CartRepository = new CartRepository(":memory:");
        }

        public void Dispose()
        {
            CartRepository.Dispose();
        }

       
    }
}
