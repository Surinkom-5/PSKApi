using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Web.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; }
        public int AvailabilityCount { get; set; }

        public static ProductViewModel ToModel(Product product)
        {
            return new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ProductType = product.ProductType.ToString(),
                AvailabilityCount = product.AvailabilityCount
            };
        }

        public static List<ProductViewModel> ToModel(List<Product> products)
        {
            return (products.Select(product => ToModel(product))).ToList();
        }
    }
}
