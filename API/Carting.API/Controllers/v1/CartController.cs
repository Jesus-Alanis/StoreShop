using Asp.Versioning;
using Carting.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net.Mime;
using System.Text.Json;

namespace Carting.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartingService _cartingService;

        public CartController(ILogger<CartController> logger, ICartingService cartingService)
        {
            _logger = logger;
            _cartingService = cartingService;
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{cartId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Cart), StatusCodes.Status200OK)]
        public IResult GetCart(string cartId)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(cartId)] = cartId
            })) 
            {
                var cart = _cartingService.GetCart(cartId);
                return Results.Ok(cart);
            }                
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{cartId}/items/{itemId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public IResult GetItem(string cartId, long itemId)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(cartId)] = cartId,
                [nameof(itemId)] = itemId
            }))
            {
                var item = _cartingService.GetItem(cartId, itemId);
                if(item == null)
                {
                    _logger.LogWarning("Cart Item Not Found");
                    Results.NotFound();
                }

                return Results.Ok(item);
            }            
        }

        [RequiredScope("manager.create, buyer.read")]
        [HttpPost("{cartId}/items")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status201Created)]
        public IResult AddItemToCart(string cartId, [FromBody] Application.DTOs.Item dto)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(cartId)] = cartId,
                ["item"] = JsonSerializer.Serialize(dto)
            }))
            {
                var itemId = _cartingService.AddItem(cartId, dto);
                var location = Url.Action(nameof(GetItem), new { cartId, dto.ItemId }) ?? $"{cartId}/items/{dto.ItemId}";
                return Results.Created(location, dto);
            }           
        }

        [RequiredScope("manager.delete, buyer.read")]
        [HttpDelete("{cartId}/items/{itemId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IResult DeleteItem(string cartId, long itemId)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(cartId)] = cartId,
                [nameof(itemId)] = itemId
            }))
            {
                _cartingService.RemoveItem(cartId, itemId);
                return Results.Ok();
            }            
        }

    }
}
