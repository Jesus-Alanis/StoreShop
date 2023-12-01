using Carting.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Carting.API.Controllers.v2
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiVersion("2.0")]
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
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Item>), StatusCodes.Status200OK)]
        public IResult GetCart(string cartId)
        {
            var cart = _cartingService.GetCart(cartId);
            return Results.Ok(cart.Items);
        }


    }
}
