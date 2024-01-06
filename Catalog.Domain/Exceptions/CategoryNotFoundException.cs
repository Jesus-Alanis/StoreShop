namespace Catalog.Domain.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(long categoryId) : base($"Category [{categoryId}] Not Found")
        {
        }

    }
}
