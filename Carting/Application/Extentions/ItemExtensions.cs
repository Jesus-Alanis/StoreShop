using Carting.Domain.Entities;

namespace Carting.Application.Extentions
{
    public static class ItemExtensions
    {
        public static Item ToEntity(this DTOs.Item dto, string cartId)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return new Item(dto.ItemId, cartId, dto.Name, dto.Price, dto.Quantity)
            {
                Image = dto.Image
            };
        }

        public static DTOs.Item ToDto(this Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new DTOs.Item(item.ItemId, item.Name, item.Price, item.Quantity)
            {
                Image = item.Image
            };
        }
    }
}
