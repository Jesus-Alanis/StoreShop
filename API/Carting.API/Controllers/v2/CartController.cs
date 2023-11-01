using Carting.Application;
using Carting.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carting.API.Controllers.v2
{

    [ApiController]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiVersion("2.0")]
    public class CartController : ControllerBase
    {
        private readonly ICartingService _cartingService;

        public CartController(ICartingService cartingService)
        {
            _cartingService = cartingService;
        }

        [HttpGet("{cartId}")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        public IResult GetCart(string cartId)
        {
            var items = _cartingService.GetCart(cartId);
            return Results.Ok(items);
        }


    }
}
