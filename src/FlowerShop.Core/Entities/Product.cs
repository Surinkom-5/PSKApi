using System.ComponentModel.DataAnnotations;
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
        [MaxLength(150)]
        public string ImageUrl { get; private set; }

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

        public void SetImageUrl(string imageUrl) => ImageUrl = imageUrl;

        public void SetAvailabilityCount(int newCount) => AvailabilityCount = newCount;
    }
}
