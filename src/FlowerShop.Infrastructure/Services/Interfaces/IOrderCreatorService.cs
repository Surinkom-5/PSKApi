using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Models;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services.Interfaces
{
    public interface IOrderCreatorStrategy
    {
        public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderModel orderModel);
    }
}