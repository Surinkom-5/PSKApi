namespace FlowerShop.Core.Entities
{
    public class FlowersBouquet
    {
        public int FlowerId { get; private set; }
        public Flower Flower { get; private set; }
        public int BouquetId { get; private set; }
        public Bouquet Bouquet { get; private set; }
        public int FlowerCount { get; private set; }

        private FlowersBouquet() 
        {
        }

    }
}
