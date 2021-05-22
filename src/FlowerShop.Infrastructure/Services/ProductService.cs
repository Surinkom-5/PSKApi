using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(string name, decimal price, string description, ProductType productType);

        public Task<bool> UpdateProductAsync(int id, string name, decimal? price, string description, int? quantity);

        public Task<bool> RemoveProductAsync(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;

        public ProductService(AppDbContext context, IProductRepository productRepository)
        {
            _dbContext = context;
            _productRepository = productRepository;
        }

        public async Task<int> CreateProductAsync(string name, decimal price, string description, ProductType productType)
        {
            var product = new Product(name, price, description, productType);
            await _dbContext.AddAsync(product);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(int id, string name, decimal? price, string description, int? quantity)
        {
            var productToUpdate = await _productRepository.GetProductByIdAsync(id);

            if (productToUpdate == null)
            {
                return false;
            }

            productToUpdate.UpdateProductDetails(name, price, description, quantity);
            _dbContext.Entry(productToUpdate).CurrentValues.SetValues(productToUpdate);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(int productId)
        {
            var productToRemove = await _productRepository.GetProductByIdAsync(productId);

            if (productToRemove == null)
            {
                return false;
            }

            _dbContext.Products.Remove(productToRemove);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}