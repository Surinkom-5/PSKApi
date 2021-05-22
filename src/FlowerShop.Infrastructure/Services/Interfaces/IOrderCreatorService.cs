using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderCreatorStrategy
    {
        public Task<Order> CreateOrderAsync(CreateOrderModel orderModel);
    }
}