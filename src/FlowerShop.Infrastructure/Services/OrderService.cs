using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Models;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderCreatorStrategy _orderCreatorStrategy;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            AppDbContext dbContext,
            IOrderRepository orderRepository,
            ILogger<OrderService> logger,
            IOrderCreatorServiceFactory _orderCreatorServiceFactory,
            IHttpContextAccessor context)
        {
            _dbContext = dbContext;
            _orderCreatorStrategy = context.HttpContext.User.Identity.IsAuthenticated
                ? _orderCreatorServiceFactory.GetOrderService(OrderServiceType.RegularOrder)
                : _orderCreatorServiceFactory.GetOrderService(OrderServiceType.AnonymousOrder);
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderModel orderModel)
        {
            return await _orderCreatorStrategy.CreateOrderAsync(orderModel);
        }

        public async Task<bool> ChangeOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                {
                    return false;
                }

                order.ChangeStatus(Enum.Parse<OrderStatus>(orderStatus));
                _dbContext.Orders.Update(order);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ChangeOrderStatus");
                return false;
            }
        }
    }
}