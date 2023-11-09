using Carting.Domain.Entities;

namespace Carting.Application.DTOs
{
    public class Cart
    {
        public string Id { get; }

        public IEnumerable<Item> Items { get; }

        public Cart(string id, IEnumerable<Item> items)
        {
            Id = id;
            Items = items;
        }
    }
}
