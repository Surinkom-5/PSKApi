using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowerShop.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(AppDbContext dbContext, IProductRepository productRepository, ILogger<ShoppingCartService> logger, IShoppingCartRepository shoppingCartRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Cart> CreateCart()
        {
            var cart = new Cart(0);
            var createdCart = await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return createdCart.Entity;
        }

        public async Task<Cart> GetUserCart(int userId)
        {
            try
            {
                // If a cart already exists - get it and return
                var existingCart = await _shoppingCartRepository.GetCartByUserId(userId);
                if (existingCart != null) return existingCart;

                // Else-wise create a new cart for given user
                var cart = new Cart(0);
                cart.SetUser(userId);
                var createdCart = await _dbContext.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
                return createdCart.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: GetUserCart");
                throw;
            }
        }

        public async Task<bool> AddItemToCart(string cartId, int itemId, int quantity)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var cart = await _dbContext.Carts.FindAsync(Guid.Parse(cartId));
                var cartItem = new CartItem(cart.Id, itemId, quantity);
                var result = await _dbContext.CartItems.AddAsync(cartItem);

                var product = await _productRepository.GetProductByIdAsync(itemId);

                if (product == null)
                {
                    return false;
                }

                cart.SetPrice(product.Price + cart.Price);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return result != null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: AddItemToCart");
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCart(string cartId, int itemId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
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
                cart.SetPrice(cart.Price - product.Price);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return result != null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in ShoppingCartService: RemoveItemFromCart");
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}