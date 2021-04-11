using System.Collections.Generic;

namespace FlowerShop.Core.Entities
{
    public class Flower
    {
        public int FlowerId { get; private set; }
        public string Flowername { get; private set; }
        public double Price { get; private set; }
        public int FlowersInStorage { get; private set; }

        private readonly List<FlowersBouquet> _flowersBouquet = new();
        public IReadOnlyCollection<FlowersBouquet> FlowersBouquet => _flowersBouquet.AsReadOnly();

        private readonly List<Cart> _carts = new();
        public IReadOnlyCollection<Cart> Carts => _carts.AsReadOnly();

        private Flower()
        {
        }

        public Flower(string flowerName)
        {
            Flowername = flowerName;
        }

    }
}
