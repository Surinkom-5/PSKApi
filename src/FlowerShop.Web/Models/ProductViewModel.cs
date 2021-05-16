using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public static ProductViewModel ToModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ProductType = product.ProductType.ToString(),
                Quantity = product.AvailabilityCount,
                Image = product.ImageUrl
            };
        }

        public static List<ProductViewModel> ToModel(List<Product> products)
        {
            return (products.Select(product => ToModel(product))).ToList();
        }
    }
}