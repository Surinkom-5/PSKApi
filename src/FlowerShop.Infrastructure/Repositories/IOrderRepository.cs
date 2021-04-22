using FlowerShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrderByIdAsync(int orderId);
    }
}
