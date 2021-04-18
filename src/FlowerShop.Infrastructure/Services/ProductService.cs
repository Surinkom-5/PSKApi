using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(string name, decimal price, string description, ProductType productType);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> CreateProductAsync(string name, decimal price, string description, ProductType productType)
        {
            var product = new Product(name, price, description, productType);
            await _dbContext.AddAsync(product);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
