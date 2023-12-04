using Carting.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net.Mime;

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
        private readonly ICartingService _cartingService;

        public CartController(ICartingService cartingService)
        {
            _cartingService = cartingService;
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{cartId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Cart), StatusCodes.Status200OK)]
        public IResult GetCart(string cartId)
        {
            var cart = _cartingService.GetCart(cartId);
            return Results.Ok(cart);
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{cartId}/items/{itemId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public IResult GetItem(string cartId, long itemId)
        {
            var item = _cartingService.GetItem(cartId, itemId);
            return Results.Ok(item);
        }

        [RequiredScope("manager.create, buyer.read")]
        [HttpPost("{cartId}/items")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status201Created)]
        public IResult AddItemToCart(string cartId, [FromBody] Application.DTOs.Item dto)
        {
            var itemId = _cartingService.AddItem(cartId, dto);
            var location = Url.Action(nameof(GetItem), new { cartId, itemId }) ?? $"{cartId}/items/{itemId}";
            return Results.Created(location, dto);
        }

        [RequiredScope("manager.delete, buyer.read")]
        [HttpDelete("{cartId}/items/{itemId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteCategory(string cartId, long itemId)
        {
            _cartingService.RemoveItem(cartId, itemId);
            return Results.Ok();
        }

    }
}
