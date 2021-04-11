using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class User
    {
        public int UserId { get; private set; }
        public string Email { get; private set; }

        private readonly List<Cart> _carts = new();
        public IReadOnlyCollection<Cart> Carts => _carts.AsReadOnly();

        private User() 
        {
        }

    }
}
