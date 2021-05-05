using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _dbContext;

        public ShoppingCartRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Cart> GetCartByPublicIdAsync(string id)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            await _dbContext.Entry(cart).Collection(c => c.CartItems).LoadAsync();
            return cart;
        }
    }
}
