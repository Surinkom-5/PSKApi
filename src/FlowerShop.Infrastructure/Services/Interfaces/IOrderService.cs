using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrder(string email, string phoneNumber, string comment, Guid cartId);

        public Task<Order> ChangeOrderStatus(int cartId);
    }
}
