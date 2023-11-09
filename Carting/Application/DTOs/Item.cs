using Carting.Domain.ValueObjects;

namespace Carting.Application.DTOs
{
    public class Item
    {
        public string Name { get; set; }
        public Image? Image { get; set; }
        public double Price { get; }
        public short Quantity { get; }

        public Item(string name, double price, short quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
