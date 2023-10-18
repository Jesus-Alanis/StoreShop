using Carting.Domain.ValueObjects;

namespace Carting.Domain.Entities
{
    public class Item
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public double Price { get; set; }
        public short Quantity { get; set; }

        public Item(long id, long cartId, string name, double price, short quantity)
        {
            Id = id;
            CartId = cartId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
