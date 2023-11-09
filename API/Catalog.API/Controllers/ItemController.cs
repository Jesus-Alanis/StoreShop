using Catalog.Application;
using Catalog.Application.Extensions;
using Catalog.Domain.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public ItemController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("categories/{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Item>), StatusCodes.Status200OK)]
        public async Task<IResult> GetItems(long categoryId, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var items = await _catalogService.GetPaginatedItemsAsync(categoryId, pageSize, pageIndex);
            return Results.Ok(items.Select(c => c.ToDto()));
        }

        [HttpGet("{itemId}")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public async Task<IResult> GetItem(long itemId)
        {
            var item = await _catalogService.GetItemAsync(itemId);
            return Results.Ok(item.ToDto());
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status201Created)]
        public async Task<IResult> CreateItem(Application.DTOs.Item dto)
        {
            var itemId = await _catalogService.AddItemAsync(dto);
            var location = Url.Action(nameof(GetItem), new { itemId }) ?? $"/{itemId}";
            return Results.Created(location, dto);
        }

        [HttpPut("{itemId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateItem(long itemId, [FromBody] Application.DTOs.Item dto)
        {
            await _catalogService.UpdateItemAsync(itemId, dto);
            return Results.Ok();
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteItem(long itemId)
        {
            await _catalogService.RemoveItemAsync(itemId);
            return Results.Ok();
        }
    }
}
