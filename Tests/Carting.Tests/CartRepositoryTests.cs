using Carting.Domain.Entities;

namespace Carting.Tests
{
    public class CartRepositoryTests : IClassFixture<DatabaseFixture>
    {

        private readonly DatabaseFixture _fixture;

        public CartRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddCartItem_PassItem_ShouldSaveItem()
        {
            var item = new Item(cartId: Guid.NewGuid().ToString(), name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 1,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var id = _fixture.CartRepository.Addtem(item);

            Assert.True(id > 0);
        }

        [Fact]
        public void RemoveCartItem_PassId_ShouldRemoveItem()
        {
            var item = new Item(cartId: Guid.NewGuid().ToString(), name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 2,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var id = _fixture.CartRepository.Addtem(item);
            var isDeleted = _fixture.CartRepository.RemoveItem(id);

            Assert.True(isDeleted);
        }

        [Fact]
        public void GetCartItems_PassCartId2_ShouldGet2items()
        {
            var item1 = new Item(cartId: Guid.NewGuid().ToString(), name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 3,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var cart2Id = Guid.NewGuid().ToString();

            var item2 = new Item(cartId: cart2Id, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 4,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var item3 = new Item(cartId: cart2Id, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 5,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            _ = _fixture.CartRepository.Addtem(item1);
            _ = _fixture.CartRepository.Addtem(item2);
            _ = _fixture.CartRepository.Addtem(item3);

            var items = _fixture.CartRepository.GetItems(cartId: cart2Id);

            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void Exists_PassId_ShouldFindItem()
        {
            var cartId = Guid.NewGuid().ToString();

            var item = new Item(cartId: cartId, name: "Lightbulb", price: 1.25, quantity: 1)
            {
                Id = 6,
                Image = new Domain.ValueObjects.Image("https://some.domain.com")
            };

            var itemId = _fixture.CartRepository.Addtem(item);
            var isFound = _fixture.CartRepository.Exists(cartId, itemId);

            Assert.True(isFound);
        }
    }
}