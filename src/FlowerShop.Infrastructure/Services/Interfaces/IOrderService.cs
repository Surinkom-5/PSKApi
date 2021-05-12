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
        public Task<Order> CreateOrder(string email, string phoneNumber, string comment, Guid cartId, string firstName, string lastName, string address, string city, string postCode);

        public Task<bool> ChangeOrderStatus(int orderId, string orderStatus);
    }
}
