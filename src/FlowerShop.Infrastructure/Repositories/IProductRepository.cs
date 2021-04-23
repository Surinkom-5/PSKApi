using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
   public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(int productId);
        public Task<List<Product>> GetProductsAsync();
    }
}
