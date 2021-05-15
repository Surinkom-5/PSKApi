using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlowerShop.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(AppDbContext dbContext, IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrder(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(orderModel.CartId.ToString());
                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == orderModel.CartId).ToListAsync();

                if (cartItems == null || cartItems.Count <= 0 || cart == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await ReduceProductAvailability(_dbContext, cartItems);

                var order = new Order(orderModel.Email, orderModel.PhoneNumber, orderModel.Comment, cart.Price, orderModel.FirstName, orderModel.LastName, orderModel.Address, orderModel.City, orderModel.PostCode);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await _dbContext.SaveChangesAsync();

                var orderItems = CartItem.ToOrderItems(cartItems, order.OrderId);
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                _dbContext.CartItems.RemoveRange(cartItems);
                cart.SetPrice(0);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();

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

        private async Task ReduceProductAvailability(AppDbContext dbContext, List<CartItem> cartItems)
        {
            try
            {
                var idsToReduce = cartItems.Select(i => i.ProductId).ToList();
                var productsToReduce = await dbContext.Products.Where(i => idsToReduce.Contains(i.ProductId)).ToListAsync();

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

                dbContext.Products.UpdateRange(productsToReduce);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ReduceProductAvailability");
                throw;
            }
        }

        public async Task<bool> ChangeOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                {
                    return false;
                }

                order.ChangeStatus(Enum.Parse<OrderStatus>(orderStatus));
                _dbContext.Orders.Update(order);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ChangeOrderStatus");
                return false;
            }
        }
    }
}
