namespace Catalog.Domain.Entities
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
        public long? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
