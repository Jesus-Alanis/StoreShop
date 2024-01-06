using Carting.gRPC.Services;

namespace Carting.gRPC.Extentions
{
    public static class ItemExtensions
    {
        public static Domain.Entities.Item ToEntity(this Item dto, string cartId)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var item = new Domain.Entities.Item(dto.ItemId, cartId, dto.Name, dto.Price, Convert.ToInt16(dto.Quantity));

            if (dto.Image != null)
                item.Image = new Domain.ValueObjects.Image(dto.Image.Url) { AltText = dto.Image.AltText };

            return item;
        }

        public static Item ToDto(this Domain.Entities.Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var dto = new Item
            {
                ItemId = item.ItemId,
                Name = item.Name,                                              
                Price = item.Price,
                Quantity = Convert.ToUInt32(item.Quantity)
            };

            if(item.Image != null)
                dto.Image = new Image { Url = item.Image.Url, AltText = item.Image.AltText };        
            
            return dto; 
        }
    }
}
