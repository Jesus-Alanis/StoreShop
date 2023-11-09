using Catalog.Domain.Entities;

namespace Catalog.Application.Extensions
{

    //TODO replace for a mapper library
    public static class CategoryExtensions
    {
        public static Category ToEntity(this DTOs.Category dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return new Category(dto.Name ?? string.Empty)
            {                
                Url = dto.Url,
                ParentCategoryId = dto.ParentCategoryId
            };
        }

        //TODO check value change
        public static void MapEntity(this DTOs.Category dto, Category category)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.Name = dto.Name;
            category.Url = dto.Url;
            category.ParentCategoryId = dto.ParentCategoryId;
        }

        public static DTOs.Category ToDto(this Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return new DTOs.Category
            {
                Name = category.Name,
                Url = category.Url,
                ParentCategoryId = category.ParentCategoryId
            };
        }

    }
}
