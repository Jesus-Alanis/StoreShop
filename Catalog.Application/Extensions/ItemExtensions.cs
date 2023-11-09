using Catalog.Domain.Entities;

namespace Catalog.Application.Extensions
{
    //TODO replace for a mapper library
    public static class ItemExtensions
    {
        public static Item ToEntity(this DTOs.Item dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return new Item(dto.Name ?? string.Empty, dto.CategoryId, dto.Price, dto.Amount)
            {
                Description = dto.Description,
                Url = dto.Url
            };
        }

        //TODO check value change
        public static void MapEntity(this DTOs.Item dto, Item item) 
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Url = dto.Url;
            item.CategoryId = dto.CategoryId;
            item.Price = dto.Price;
            item.Amount = dto.Amount;        
        }

        public static DTOs.Item ToDto(this Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new DTOs.Item
            {
                Name = item.Name,
                Description = item.Description,
                Url = item.Url,
                CategoryId = item.CategoryId,
                Price = item.Price,
                Amount = item.Amount
            };
        }
    }
}
