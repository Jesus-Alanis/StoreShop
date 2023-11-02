namespace Carting.Infra.ExternalServices.DTOs
{
    internal class Item
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public double Price { get; set; }
    }
}
