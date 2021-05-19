using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
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

        public RegularOrderCreatorStrategy(
            AppDbContext dbContext,
            IShoppingCartRepository shoppingCartRepository,
            ILogger<AnonymousOrderCreatorStrategy> logger)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Get cart by user Id
                var cart = await _shoppingCartRepository.GetCartByUserId(orderModel.UserId.Value);
                if (cart is null)
                {
                    throw new ArgumentNullException(nameof(cart));
                }

                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == cart.Id).ToListAsync();
                if (cartItems == null || cartItems.Count <= 0)
                {
                    throw new ArgumentException("Cart items must can't be empty or null");
                }

                await ReduceProductAvailability(cartItems);

                var order = new Order(orderModel.UserId.Value, orderModel.AddressId.Value, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    throw new ArgumentNullException(nameof(createdOrder.Entity));
                }

                await _dbContext.SaveChangesAsync();

                await UpdateOrderItemsFromCart(cartItems, order.OrderId, cart);

                await transaction.CommitAsync();
                return createdOrder.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: CreateOrder");
                await transaction.RollbackAsync();
                return null;
            }
        }

        private async Task ReduceProductAvailability(List<CartItem> cartItems)
        {
            try
            {
                var idsToReduce = cartItems.Select(i => i.ProductId).ToList();
                var productsToReduce = await _dbContext.Products.Where(i => idsToReduce.Contains(i.ProductId)).ToListAsync();

                productsToReduce.ForEach(p =>
                {
                    var reducedAvailability = p.AvailabilityCount - cartItems.First(i => i.ProductId == p.ProductId).Quantity;
                    if (reducedAvailability < 0)
                    {
                        throw new InvalidOperationException(
                            $"Attempting to buy more of Product with id {p.ProductId} than is currently available.\n");
                    }

                    p.SetAvailabilityCount(reducedAvailability);
                });

                _dbContext.Products.UpdateRange(productsToReduce);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ReduceProductAvailability");
                throw;
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