using System;
using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public decimal Price { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        private Cart()
        {
        }

        public Cart(decimal price)
        {
            Price = price;
            Id = Guid.NewGuid();
        }

        public void SetPrice(decimal price) => Price = price;
    }
}
