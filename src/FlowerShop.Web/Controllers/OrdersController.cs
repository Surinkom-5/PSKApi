using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderRepository _orderReprository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository orderRepository,
            ILogger<OrdersController> logger)
        {
            _orderReprository = orderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets order by provided order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByOrderIdAsync([FromRoute] int orderId)
        {
            try
            {
                var order = await _orderReprository.GetOrderByIdAsync(orderId);

                return order is null ? NotFound() : Ok(OrderViewModel.ToModel(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get order by order id");
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets order list by provided user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserIdAsync([FromQuery] int userId)
        {
            try
            {
                var order = await _orderReprository.GetOrdersByUserIdAsync(userId);

                return order is null ? NotFound() : Ok(OrderViewModel.ToModel(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get orders by user id");
                return BadRequest();
            }
        }
    }
}