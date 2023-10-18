using Carting.Domain.Entities;
using Carting.Domain.Repositories;
using LiteDB;

namespace Carting.DataAccess.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private bool _disposedValue;
        private ILiteDatabase _database;
        private readonly ILiteCollection<Item> _collection;

        public CartRepository(string connectionString)
        {
            _database = new LiteDatabase(connectionString);
            _collection = _database.GetCollection<Item>("cart_items");
        }

        public List<Item> GetCartItems(long cartId)
        {
            return _collection.Query().Where(item => item.CartId == cartId).ToList();
        }

        public void AddCartItem(Item item)
        {
            var id = _collection.Insert(item);
            _collection.EnsureIndex(i => i.CartId);
            _collection.EnsureIndex(i => i.Id);
        }

        public void RemoveCartItem(long itemId)
        {
            _collection.Delete(itemId);
        }

        public bool Exists(long itemId)
        {
            return _collection.Exists(item => item.Id == itemId);
        }

        ~CartRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _database.Dispose();
                    _database = null;
                }
                _disposedValue = true;
            }
        }
    }
}
