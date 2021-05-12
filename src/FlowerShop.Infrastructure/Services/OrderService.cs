using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderService(AppDbContext dbContext, IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _dbContext = dbContext;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Order> CreateOrder(string email, 
            string phoneNumber, 
            string comment, 
            Guid cartId, 
            string firstName, 
            string lastName, 
            string address, 
            string city, 
            string postCode)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(cartId.ToString());
                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == cartId).ToListAsync();

                if (cartItems == null || cartItems.Count <= 0 || cart == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                var idsToReduce = cartItems.Select(i => i.ProductId).ToList();
                var productsToReduce = await _dbContext.Products.Where(i => idsToReduce.Contains(i.ProductId)).ToListAsync();

                productsToReduce.ForEach(p => p.SetAvailabilityCount(p.AvailabilityCount - cartItems.First(i => i.ProductId == p.ProductId).Quantity));

                _dbContext.Products.UpdateRange(productsToReduce);

                var order = new Order(email, phoneNumber, comment, cart.Price, firstName, lastName, address, city, postCode);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await _dbContext.SaveChangesAsync();

                var orderItems = CartItem.ToOrderItems(cartItems, createdOrder.Entity.OrderId);
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                _dbContext.CartItems.RemoveRange(cartItems);
                cart.Price = 0;
                _dbContext.Carts.Update(cart);
                var result = await _dbContext.SaveChangesAsync();

                if (result < 0)
                {
                    return null;
                }

                await transaction.CommitAsync();
                return createdOrder.Entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
