using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
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

        public async Task<Order> CreateOrder(string email, string phoneNumber, string comment, Guid cartId)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(cartId.ToString());
                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == cartId).ToListAsync();

                if (cartItems == null || cartItems.Count < 0 || cart == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                var order = new Order(email, phoneNumber, comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await _dbContext.SaveChangesAsync();
                // TODO: subtract quantity of these items

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

        public Task<Order> ChangeOrderStatus(int cartId)
        {
            throw new NotImplementedException();
        }
    }
}
