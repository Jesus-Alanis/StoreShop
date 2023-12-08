using Carting.Domain.Entities;
using Carting.Domain.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Carting.DataAccess.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private const string COLLECTION_NAME = "cart_items";
        private ILiteDatabase? _database;
        private readonly ILiteCollection<Item> _collection;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(ILogger<CartRepository> logger, string connectionString)
        {
            _logger = logger;
            _database = new LiteDatabase(connectionString);
            _collection = _database.GetCollection<Item>(COLLECTION_NAME);          
        }

        public List<Item> GetItems(string cartId)
        {
            _logger.LogInformation(string.Format("Querying {0} collection", COLLECTION_NAME));
            return _collection.Query().Where(item => item.CartId == cartId).ToList();
        }

        public Item GetItem(string cartId, long itemId)
        {
            _logger.LogInformation(string.Format("Querying {0} collection", COLLECTION_NAME));
            return _collection.FindOne(item => item.CartId == cartId && item.ItemId == itemId);
        }

        public int UpdateItems(long itemId, string name, string url, double price)
        {
            _logger.LogInformation(string.Format("Querying {0} collection", COLLECTION_NAME));
            var items = _collection.Query().Where(item => item.ItemId == itemId).ToList();
            if (!items.Any())
                return 0;

            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(name) && item.Name != name)
                    item.Name = name;

                if (price > 0 && item.Price != price)
                    item.Price = price;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (item.Image is null)
                    {
                        item.Image = new Domain.ValueObjects.Image(url);
                    }
                    else
                    {
                        if (item.Image.Url != url)
                            item.Image.Url = url;
                    }                                  
                }                  
            }

            _logger.LogInformation(string.Format("Updating {0} collection", COLLECTION_NAME));
            return _collection.Update(items);
        }

        public long Addtem(Item item)
        {
            _logger.LogInformation(string.Format("Inserting into {0} collection", COLLECTION_NAME));
            var cartItemId = _collection.Insert(item).AsInt64;
            _collection.EnsureIndex(i => i.CartId);
            _collection.EnsureIndex(i => i.ItemId);

            return cartItemId;
        }

        public bool RemoveItem(long cartItemId)
        {
            _logger.LogInformation(string.Format("Deleting from {0} collection", COLLECTION_NAME));
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
