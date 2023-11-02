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

        public List<Item> GetItems(string cartId)
        {
            return _collection.Query().Where(item => item.CartId == cartId).ToList();
        }

        public Item GetItem(string cartId, long itemId)
        {
            return _collection.FindOne(item => item.CartId == cartId && item.ItemId == itemId);
        }

        public int UpdateItems(long itemId, string name, string url, double price)
        {
            var items = _collection.Query().Where(item => item.ItemId == itemId).ToList();
            if (!items.Any())
                return 0;

            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(url) && item.Name != name)
                    item.Name = name;

                if (!string.IsNullOrWhiteSpace(url) && item.Price != price)
                    item.Price = price;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (item.Image is null)
                        item.Image = new Domain.ValueObjects.Image(url);

                    if(item.Image.Url != url)
                        item.Image.Url = url;
                }                  
            }

            return _collection.Update(items);
        }

        public long Addtem(Item item)
        {
            var cartItemId = _collection.Insert(item).AsInt64;
            _collection.EnsureIndex(i => i.CartId);
            _collection.EnsureIndex(i => i.ItemId);

            return cartItemId;
        }

        public bool RemoveItem(long cartItemId)
        {
            return _collection.Delete(cartItemId);
        }

        public bool Exists(string cartId, long itemId)
        {
            return _collection.Exists(item => item.CartId == cartId && item.ItemId == itemId);
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
