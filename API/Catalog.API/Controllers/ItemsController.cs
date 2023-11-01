using Catalog.Application;
using Catalog.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public ItemsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Item>), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategories()
        {
            var items = await _catalogService.GetItemsAsync().ConfigureAwait(false);
            return Results.Ok(items.Select(c => c.ToDto()).AsEnumerable());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public async Task<IResult> GetItem(long id)
        {
            var item = await _catalogService.GetItemAsync(id).ConfigureAwait(false);
            return Results.Ok(item.ToDto());
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status201Created)]
        public async Task<IResult> CreateItem(Application.DTOs.Item dto)
        {
            var itemId = await _catalogService.AddItemAsync(dto).ConfigureAwait(false);
            var location = Url.Action(nameof(GetItem), new { id = itemId }) ?? $"/{itemId}";
            return Results.Created(location, dto);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateItem(long id, [FromBody] Application.DTOs.Item dto)
        {
            await _catalogService.UpdateItemAsync(id, dto).ConfigureAwait(false);
            return Results.Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteItem(long id)
        {
            await _catalogService.RemoveItemAsync(id).ConfigureAwait(false);
            return Results.Ok();
        }
    }
}
