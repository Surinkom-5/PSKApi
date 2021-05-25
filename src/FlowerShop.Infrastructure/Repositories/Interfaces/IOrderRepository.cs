using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrderByIdAsync(int orderId);

        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        public Task<List<Order>> GetAllOrders();
    }
}