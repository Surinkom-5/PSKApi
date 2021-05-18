using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrder(CreateOrderModel orderModel);

        public Task<Order> CreateOrderForAuthenticatedUser(CreateOrderModel orderModel);

        public Task<bool> ChangeOrderStatus(int orderId, string orderStatus);
    }
}
