using FlowerShop.Core.Enums;

namespace FlowerShop.Core.Entities
{
    public class Product
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int AvailabilityCount { get; private set; }
        public ProductType ProductType { get; private set; }

        private Product()
        {
        }

        public Product(string name, decimal price, string description, ProductType productType)
        {
            Name = name;
            Price = price;
            Description = description;
            ProductType = productType;
        }

        public void SetAvailabilityCount(int newCount) => AvailabilityCount = newCount;

        public void UpdateProductDetails(string name, decimal? price, string description, int? quantity)
        {
            if(name != null)
            {
                Name = name;
            }

            if(price.HasValue)
            {
                Price = (decimal)price;
            }

            if(description != null)
            {
                Description = description;
            }

            if(quantity.HasValue)
            {
                AvailabilityCount = (int)quantity;
            }
        }
    }
}
