using Catalog.Domain.Entities;

namespace Catalog.Application.Extensions
{
    //TODO repalce for a mapper library
    public static class ItemExtensions
    {
        public static Item ToEntity(this DTOs.Item dto)
        {
            return new Item(dto.Name ?? string.Empty, dto.CategoryId, dto.Price, dto.Amount)
            {
                Description = dto.Description,
                Url = dto.Url
            };
        }

        //TODO check value change
        public static void MapEntity(this DTOs.Item dto, Item item) 
        {
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Url = dto.Url;
            item.CategoryId = dto.CategoryId;
            item.Price = dto.Price;
            item.Amount = dto.Amount;        
        }
    }
}
