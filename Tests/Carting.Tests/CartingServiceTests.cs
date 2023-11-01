﻿using Carting.Application;
using Carting.Domain.Entities;
using Carting.Domain.Exceptions;

namespace Carting.Tests
{
    public class CartingServiceTests : IClassFixture<DatabaseFixture>
    {

        private readonly ICartingService _cartingService;

        public CartingServiceTests(DatabaseFixture fixture)
        {
            _cartingService = new CartingService(fixture.CartRepository);
        }

        [Fact]
        public void AddCartItem_PassItem_ShouldSaveItem()
        {         
            var item = new Application.DTOs.Item(name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var id = _cartingService.AddItem(cartId: Guid.NewGuid().ToString(), item);

            Assert.True(id > 0);
        }        

        [Fact]
        public void RemoveCartItem_PassId_ShouldRemoveItem()
        {
            var item = new Application.DTOs.Item(name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var cartId = Guid.NewGuid().ToString();

            var id = _cartingService.AddItem(cartId, item);
            var isDeleted = _cartingService.RemoveItem(cartId, id);

            Assert.True(isDeleted);
        }

        [Fact]
        public void GetCartItems_PassCartId2_ShouldGet2items()
        {
            var item1 = new Application.DTOs.Item(name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var cart2Id = Guid.NewGuid().ToString();

            var item2 = new Application.DTOs.Item(name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var item3 = new Application.DTOs.Item(name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            _ =  _cartingService.AddItem(cartId: Guid.NewGuid().ToString(), item1);
            _ =  _cartingService.AddItem(cartId: cart2Id, item2);
            _ = _cartingService.AddItem(cartId: cart2Id, item3);

            var cart = _cartingService.GetCart(cartId: cart2Id);

            Assert.Equal(2, cart.Items.Count());
        }

        [Fact]
        public void RemoveCartItem_PassInvalidId_ShouldThrowException()
        {
            Assert.Throws<ItemNotFoundException>(() => _cartingService.RemoveItem(cartId: Guid.NewGuid().ToString(), itemId: 7));
        }
    }
}