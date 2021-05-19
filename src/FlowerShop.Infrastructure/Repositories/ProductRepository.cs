using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Helpers;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<ProductType>> GetAvailableProductTypesAsync()
        {
            return await _dbContext.Products.GroupBy(x => x.ProductType).Select(x => x.Key).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<Product>> GetProductsByFiltersAsync(string searchText, ProductType? productType)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = SearchQueryHelper.FormatSearchQuery(searchText);
            }

            return await _dbContext.Products
                .Where(x => (searchText == null || EF.Functions.Contains(x.Name, searchText)) &&
                            (productType == null || x.ProductType == productType))
                .ToListAsync();
        }
    }
}