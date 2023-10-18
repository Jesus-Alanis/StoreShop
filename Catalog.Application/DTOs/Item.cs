namespace Catalog.Application.DTOs
{
    public class Item
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public long CategoryId { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
