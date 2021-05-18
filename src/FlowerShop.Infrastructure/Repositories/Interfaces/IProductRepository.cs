using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(int productId);

        public Task<List<Product>> GetProductsByFiltersAsync(string searchText, ProductType? productType);

        public Task<List<ProductType>> GetAvailableProductTypesAsync();
    }
}