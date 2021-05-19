using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Infrastructure.Services
{
    public interface IOrderCreatorServiceFactory
    {
        public IOrderCreatorStrategy GetOrderService(OrderServiceType orderServiceType);
    }

    public class OrderCreatorServiceFactory : IOrderCreatorServiceFactory
    {
        private readonly IEnumerable<IOrderCreatorStrategy> _strategies;

        public OrderCreatorServiceFactory(
            IServiceProvider serviceProvider)
        {
            _strategies = serviceProvider.GetServices<IOrderCreatorStrategy>();
        }

        public IOrderCreatorStrategy GetOrderService(OrderServiceType orderServiceType)
        {
            return _strategies.First(x => x.GetType().Name.StartsWith(orderServiceType.ToString()));
        }
    }
}