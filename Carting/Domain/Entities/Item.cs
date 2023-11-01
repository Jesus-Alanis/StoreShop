using Carting.Domain.ValueObjects;

namespace Carting.Domain.Entities
{
    public class Item
    {
        /// <summary>
        /// Cart Item Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Product Id
        /// </summary>
        public long ItemId { get; set; }
        public string CartId { get; set; }        
        public string Name { get; set; }
        public Image? Image { get; set; }
        public double Price { get; set; }
        public short Quantity { get; set; }

        public Item(long itemId, string cartId, string name, double price, short quantity)
        {
            ItemId = itemId;
            CartId = cartId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
