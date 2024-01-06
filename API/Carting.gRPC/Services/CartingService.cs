using Carting.Domain.Repositories;
using Carting.gRPC.Extentions;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using System.Text.Json;

namespace Carting.gRPC.Services
{
    [Authorize]
    public class CartingService : Carting.CartingBase
    {
        private readonly ILogger<CartingService> _logger;
        private readonly ICartRepository _cartRepository;

        public CartingService(ILogger<CartingService> logger, ICartRepository cartRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
        }

        [RequiredScope("manager.read, buyer.read")]
        public override Task<CartResponse> GetItemsUnaryCall(Cart request, ServerCallContext context)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(Cart.CartId)] = request.CartId
            });

            return Task.FromResult(GetCart(request.CartId));
        }

        [RequiredScope("manager.read, buyer.read")]
        public async override Task GetItemsServerStreaming(Cart request, IServerStreamWriter<Item> responseStream, ServerCallContext context)
        {
            using var disp = _logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(Cart.CartId)] = request.CartId
            });

            var items = _cartRepository.GetItems(request.CartId);

            foreach (var item in items)
            {
                if (context.CancellationToken.IsCancellationRequested)
                    return;

                await responseStream.WriteAsync(item.ToDto());
            }
        }

        [RequiredScope("manager.create, buyer.read")]
        public async override Task<CartResponse> AddItemClientStreaming(IAsyncStreamReader<ItemRequest> requestStream, ServerCallContext context)
        {
            var cartId = string.Empty;
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (request == null)
                    continue;

                cartId = request.CartId;

                using var disp = _logger.BeginScope(new Dictionary<string, object>
                {
                    [nameof(ItemRequest.CartId)] = cartId,
                    ["item"] = JsonSerializer.Serialize(request.Item)
                });

                AddItem(request.CartId, request.Item);
            }

            return GetCart(cartId);
        }

        [RequiredScope("manager.create, buyer.read")]
        public async override Task AddItemDuplexStreaming(IAsyncStreamReader<ItemRequest> requestStream, IServerStreamWriter<CartResponse> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (request == null)
                    continue;

                using var disp = _logger.BeginScope(new Dictionary<string, object>
                {
                    [nameof(ItemRequest.CartId)] = request.CartId,
                    ["item"] = JsonSerializer.Serialize(request.Item)
                });

                AddItem(request.CartId, request.Item);
                await responseStream.WriteAsync(GetCart(request.CartId));
            }
        }

        private CartResponse GetCart(string cartId)
        {
            var items = _cartRepository.GetItems(cartId);
            var cartResponse = new CartResponse { CartId = cartId };
            cartResponse.Items.AddRange(items.Select(i => i.ToDto()));

            return cartResponse;
        }

        private void AddItem(string cartId, Item dto)
        {
            var item = dto.ToEntity(cartId);
            _cartRepository.Addtem(item);
        }
    }
}
