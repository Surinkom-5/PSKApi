using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Models;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CreateOrderResponse> CreateOrder(CreateOrderModel orderModel);

        public Task<bool> ChangeOrderStatus(int orderId, string orderStatus);
    }
}