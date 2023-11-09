namespace Carting.Domain.Entities
{
    public class Cart
    {
        public string Id { get; set; }

        public Cart(string id)
        {
            Id = id;
        }
    }
}
