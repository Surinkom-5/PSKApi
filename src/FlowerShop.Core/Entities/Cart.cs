using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Cart
    {
        public int CartId { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }
        public double Price { get; private set; }

        private readonly List<Bouquet> _bouquets = new();
        public IReadOnlyCollection<Bouquet> Bouquets => _bouquets.AsReadOnly();

        private readonly List<Flower> _flowers = new();
        public IReadOnlyCollection<Flower> Flowers => _flowers.AsReadOnly();

        private readonly List<Cart> _carts = new();
        public IReadOnlyCollection<Cart> Carts => _carts.AsReadOnly();


        private Cart() 
        {
        }

    }
}
