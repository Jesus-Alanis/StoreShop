namespace Catalog.Domain.Entities
{
    public class Item
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public long CategoryId { get; set; }
        public Category? Category { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }

        public Item(string name, long categoryId, double price, int amount)
        {
            Name = name;
            CategoryId = categoryId;
            Price = price;
            Amount = amount;
        }
    }
}
