﻿using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrder(CreateOrderModel orderModel);

        public Task<bool> ChangeOrderStatus(int orderId, string orderStatus);
    }
}