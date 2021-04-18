using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Cart
    {
        public int CartId { get; private set; }
        public double Price { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }

        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        private Cart() 
        {
        }

    }
}
