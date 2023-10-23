using Carting.Domain.Entities;
using Carting.Domain.Repositories;
using LiteDB;

namespace Carting.DataAccess.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private ILiteDatabase? _database;
        private readonly ILiteCollection<Item> _collection;

        public CartRepository(string connectionString)
        {
            _database = new LiteDatabase(connectionString);
            _collection = _database.GetCollection<Item>("cart_items");
        }

        public List<Item> GetItems(long cartId)
        {
            return _collection.Query().Where(item => item.CartId == cartId).ToList();
        }

        public long Addtem(Item item)
        {
            var id = _collection.Insert(item);
            _collection.EnsureIndex(i => i.CartId);
            _collection.EnsureIndex(i => i.Id);

            return id.AsInt64;
        }

        public bool RemoveItem(long itemId)
        {
            return _collection.Delete(itemId);
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
            if (disposing)
            {
                var db = Interlocked.Exchange(ref _database, null);
                db?.Dispose();
            }
        }
    }
}
