using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(string name, decimal price, string description, ProductType productType);

        public Task<UpdateProductResponse> UpdateProductAsync(int id, string name, decimal? price, string description, int? quantity,
            byte[] version);

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

        public async Task<UpdateProductResponse> UpdateProductAsync(int id, string name, decimal? price, string description, int? quantity,
            byte[] version)
        {
            var productToUpdate = await _productRepository.GetProductByIdAsync(id);
            if (productToUpdate == null)
            {
                return new UpdateProductResponse(false, "Product not found");
            }
            try
            {
                productToUpdate.UpdateProductDetails(name, price, description, quantity);
                _dbContext.Entry(productToUpdate).CurrentValues.SetValues(productToUpdate);

                _dbContext.Entry(productToUpdate).OriginalValues[nameof(productToUpdate.Timestamp)] = version;
                await _dbContext.SaveChangesAsync();
                return new UpdateProductResponse();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return new UpdateProductResponse(true, "Unable to save changes. Product was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Product)databaseEntry.ToObject();
                    productToUpdate.SetTimeStamp(databaseValues.Timestamp);
                    return new UpdateProductResponse(true, "The product you attempted to edit "
                        + "was modified by another user after you got the original value.");
                }
            }
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