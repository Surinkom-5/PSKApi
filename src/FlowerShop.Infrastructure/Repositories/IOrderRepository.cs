using FlowerShop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrderByIdAsync(int orderId);

        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    }
}
