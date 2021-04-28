using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Helpers;
using FlowerShop.Infrastructure.Services.Interfaces;

namespace FlowerShop.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _dbContext;

        public ShoppingCartService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Cart> CreateCart()
        {
            var cart = new Cart(0, null, Guid.NewGuid());
            var createdCart = await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return createdCart.Entity;
        }
    }
}
