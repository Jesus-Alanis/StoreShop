using Catalog.Domain.Entities;

namespace Catalog.Application.Extensions
{

    //TODO repalce for a mapper library
    public static class CategoryExtensions
    {
        public static Category ToEntity(this DTOs.Category dto)
        {
            return new Category(dto.Name ?? string.Empty)
            {                
                Url = dto.Url,
                ParentCategoryId = dto.ParentCategoryId
            };
        }

        //TODO check value change
        public static void MapEntity(this DTOs.Category dto, Category category)
        {
            category.Name = dto.Name;
            category.Url = dto.Url;
            category.ParentCategoryId = dto.ParentCategoryId;
        }

    }
}
