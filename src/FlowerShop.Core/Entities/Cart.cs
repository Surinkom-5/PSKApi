using System;
using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Cart
    {
        public int CartId { get; private set; }
        public decimal Price { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }
        public Guid PublicId { get; private set; }

        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        private Cart() 
        {
        }

        public Cart(decimal price, int? userId, Guid publicId)
        {
            Price = price;
            UserId = userId;
            PublicId = publicId;
        }
    }
}
