using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(int productId);
        public Task<List<Product>> GetProductsAsync();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }
    }
}
