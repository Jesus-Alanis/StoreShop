using Carting.Domain.Entities;

namespace Carting.Application.DTOs
{
    public class Cart
    {
        public string Id { get; set; }

        public IEnumerable<Item> Items { get; set; }

        public Cart(string id, IEnumerable<Item> items)
        {
            Id = id;
            Items = items;
        }
    }
}
