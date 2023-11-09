﻿using Carting.Domain.ValueObjects;

namespace Carting.Domain.Entities
{
    public class Item
    {
        public long Id { get; set; }
        public string CartId { get; set; }
        public string Name { get; set; }
        public Image? Image { get; set; }
        public double Price { get; set; }
        public short Quantity { get; set; }

        public Item(string cartId, string name, double price, short quantity)
        {
            CartId = cartId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
