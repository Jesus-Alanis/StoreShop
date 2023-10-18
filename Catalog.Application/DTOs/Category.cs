namespace Catalog.Application.DTOs
{
    public class Category
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
