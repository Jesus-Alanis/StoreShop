using Carting.Application;
using Carting.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Carting.API.Controllers.v1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiVersion("1.0")]
    public class CartController : ControllerBase
    {
        private readonly ICartingService _cartingService;

        public CartController(ICartingService cartingService)
        {
            _cartingService = cartingService;
        }

        [HttpGet("{cartId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Cart), StatusCodes.Status200OK)]
        public IResult GetCart(string cartId)
        {
            var cart = _cartingService.GetCart(cartId);
            return Results.Ok(cart);
        }

        [HttpGet("{cartId}/items/{itemId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Application.DTOs.Item), StatusCodes.Status200OK)]
        public IResult GetItem(string cartId, long itemId)
        {
            var item = _cartingService.GetItem(cartId, itemId);
            return Results.Ok(item);
        }

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
