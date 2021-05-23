using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Models;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public class RegularOrderCreatorStrategy : IOrderCreatorStrategy
    {
        private readonly AppDbContext _dbContext;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<AnonymousOrderCreatorStrategy> _logger;
        private readonly ICartItemService _cartItemService;

        public RegularOrderCreatorStrategy(
            AppDbContext dbContext,
            IShoppingCartRepository shoppingCartRepository,
            ILogger<AnonymousOrderCreatorStrategy> logger,
            ICartItemService cartItemService)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _cartItemService = cartItemService;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Get cart by user Id
                var cart = await _shoppingCartRepository.GetCartWithItemsByUserIdAsync(orderModel.UserId.Value);
                if (cart is null)
                {
                    throw new ArgumentNullException(nameof(cart));
                }
                if (cart.CartItems == null || cart.CartItems.Count <= 0)
                {
                    return new CreateOrderResponse("Cart items cannot be empty.");
                }

                await _cartItemService.ReduceProductAvailabilityAsync(cart.CartItems.ToList());

                var order = new Order(orderModel.UserId.Value, orderModel.AddressId.Value, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    throw new ArgumentNullException(nameof(createdOrder.Entity));
                }

                await _dbContext.SaveChangesAsync();

                await UpdateOrderItemsFromCart(cart.CartItems.ToList(), order.OrderId, cart);

                await transaction.CommitAsync();
                return new CreateOrderResponse(createdOrder.Entity.OrderId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: CreateOrder");
                await transaction.RollbackAsync();
                return null;
            }
        }

        private async Task UpdateOrderItemsFromCart(List<CartItem> cartItems, int orderId, Cart cart)
        {
            try
            {
                var orderItems = CartItem.ToOrderItems(cartItems, orderId);
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                _dbContext.CartItems.RemoveRange(cartItems);
                cart.SetPrice(0);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: UpdateOrderItemsFromCart");
                throw;
            }
        }
    }
}