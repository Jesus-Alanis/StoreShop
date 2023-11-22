using Carting.Domain.ValueObjects;

namespace Carting.Application.DTOs
{
    public class Item
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public long ItemId { get; }
        public string Name { get; }
        public Image? Image { get; set; }
        public double Price { get; }
        public short Quantity { get; }

        public Item(long itemId, string name, double price, short quantity)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
