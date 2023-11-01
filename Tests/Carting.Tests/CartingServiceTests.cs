using Carting.Application;
using Carting.Domain.Entities;

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
            var item = new Item(id: 1, cartId: 1, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var id = _cartingService.AddItem(item);

            Assert.True(id > 0);
        }        

        [Fact]
        public void RemoveCartItem_PassId_ShouldRemoveItem()
        {
            var item = new Item(id: 2, cartId: 1, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var id = _cartingService.AddItem(item);
            var isDeleted = _cartingService.RemoveItem(id);

            Assert.True(isDeleted);
        }

        [Fact]
        public void GetCartItems_PassCartId2_ShouldGet2items()
        {
            var item1 = new Item(id: 3, cartId: 1, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var item2 = new Item(id: 4, cartId: 2, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var item3 = new Item(id: 5, cartId: 2, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            _ =  _cartingService.AddItem(item1);
            _ =  _cartingService.AddItem(item2);
            _ = _cartingService.AddItem(item3);

            var items = _cartingService.GetItems(cartId: 2);

            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void AddCartItem_PassDuplicatedItem_ShouldThrowException()
        {
            var item = new Item(id: 6, cartId: 1, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            _ = _cartingService.AddItem(item);

            Assert.Throws<Exception>(() => _cartingService.AddItem(item));
        }

        [Fact]
        public void RemoveCartItem_PassInvalidId_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => _cartingService.RemoveItem(itemId: 7));
        }
    }
}
