using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

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
            var cart = new Cart(0);
            var createdCart = await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return createdCart.Entity;
        }

        public async Task<bool> AddItemToCart(string cartId, int itemId, int quantity)
        {
            var cart = await _dbContext.Carts.FindAsync(cartId);
            var cartItem = new CartItem(cart.CartId, itemId, quantity);
            var result = await _dbContext.CartItems.AddAsync(cartItem);
            await _dbContext.SaveChangesAsync();
            return result != null;
        }

        public async Task<bool> RemoveItemFromCart(string cartId, int itemId)
        {
            var cart = await _dbContext.Carts.FindAsync(cartId);
            var itemToDelete = cart.CartItems.FirstOrDefault(i => i.ProductId == itemId);

            if (itemToDelete == null)
            {
                return false;
            }

            var result = _dbContext.CartItems.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
            return result != null;
        }
    }
}
