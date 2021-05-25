using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(c => c.OrderItems)
                .FirstOrDefaultAsync(c => c.OrderId == orderId);

            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                .Include(c => c.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _dbContext.Orders
                .Include(c => c.OrderItems)
                .ToListAsync();
        }
    }
}