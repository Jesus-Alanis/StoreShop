using Catalog.Application;
using Catalog.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net.Mime;
using System.Text.Json;

namespace Catalog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/items")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly ICatalogService _catalogService;

        public ItemController(ILogger<ItemController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("categories/{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Item>), StatusCodes.Status200OK)]
        public async Task<IResult> GetItems(long categoryId, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(categoryId)] = categoryId,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex
            });

            var items = await _catalogService.GetPaginatedItemsAsync(categoryId, pageSize, pageIndex);
            return Results.Ok(items.Select(c => c.ToDto()));
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{itemId}")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public async Task<IResult> GetItem(long itemId)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(itemId)] = itemId
            });

            var item = await _catalogService.GetItemAsync(itemId);
            if (item == null)
            {
                _logger.LogWarning("Item not found");
                return Results.NotFound();
            }
            return Results.Ok(item.ToDto());
        }

        [RequiredScope("manager.create")]
        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status201Created)]
        public async Task<IResult> CreateItem(Application.DTOs.Item dto)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                ["item"] = JsonSerializer.Serialize(dto)
            });

            var itemId = await _catalogService.AddItemAsync(dto);
            var location = Url.Action(nameof(GetItem), new { itemId }) ?? $"/{itemId}";
            return Results.Created(location, dto);
        }

        [RequiredScope("manager.update")]
        [HttpPut("{itemId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateItem(long itemId, [FromBody] Application.DTOs.Item dto, [FromHeader(Name = "x-correlation-id")] string? correlationId = null)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(itemId)] = itemId,
                ["item"] = JsonSerializer.Serialize(dto)
            });

            await _catalogService.UpdateItemAsync(itemId, dto, correlationId);
            return Results.Ok();
        }

        [RequiredScope("manager.delete")]
        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteItem(long itemId)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(itemId)] = itemId
            });

            await _catalogService.RemoveItemAsync(itemId);
            return Results.Ok();
        }
    }
}
