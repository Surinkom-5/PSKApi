using System;
using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Infrastructure.Repositories.Interfaces;

namespace FlowerShop.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;

        public ShoppingCartService(AppDbContext dbContext, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
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
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var cart = await _dbContext.Carts.FindAsync(Guid.Parse(cartId));
                var cartItem = new CartItem(cart.Id, itemId, quantity);
                var result = await _dbContext.CartItems.AddAsync(cartItem);

                var product = await _productRepository.GetProductByIdAsync(itemId);
                cart.Price += product.Price;
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return result != null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCart(string cartId, int itemId)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var cart = await _dbContext.Carts.FindAsync(Guid.Parse(cartId));
                var itemToDelete = _dbContext.CartItems.FirstOrDefault(i => i.CartId == Guid.Parse(cartId) && i.ProductId == itemId);

                if (itemToDelete == null)
                {
                    return false;
                }

                var result = _dbContext.CartItems.Remove(itemToDelete);
                var product = await _productRepository.GetProductByIdAsync(itemId);
                cart.Price -= product.Price;
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return result != null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
