﻿using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Core.Constants;
using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Services.Interfaces;
using FlowerShop.Web.Extensions;
using FlowerShop.Web.Patch;
using FlowerShop.Web.Post;
using Microsoft.Extensions.Primitives;

namespace FlowerShop.Web.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository orderRepository,
            ILogger<OrdersController> logger,
            IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderService = orderService;
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
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

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
                var order = await _orderRepository.GetOrdersByUserIdAsync(userId);

                return order is null ? NotFound() : Ok(OrderViewModel.ToModel(order));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Order Controller, get orders by user id");
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates an order from the provided cartId (via cartCookie header)
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModel body)
        {
            try
            {
                // If user has "cartCookie" header value
                if (!Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues))
                {
                    // Means user does not have a cart yet
                    return StatusCode(500);
                }

                var cartCookie = headerValues.FirstOrDefault();

                if (cartCookie == null)
                {
                    return BadRequest();
                }

                Order result;
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    result = await _orderService.CreateOrderForAuthenticatedUser(new Infrastructure.CustomModels.CreateOrderModel()
                    {
                        UserId = User.Identity.GetUserId(),
                        AddressId = body.AddressId
                    });
                }
                else
                {
                    result = await _orderService.CreateOrder(new Infrastructure.CustomModels.CreateOrderModel()
                    {
                        Email = body.Email,
                        Comment = body.Comment,
                        CartId = Guid.Parse(cartCookie),
                        FirstName = body.FirstName,
                        LastName = body.LastName,
                        Address = body.Address,
                        City = body.City,
                        PostCode = body.PostCode,
                        PhoneNumber = body.PhoneNumber
                    });
                }

                return result is null ? StatusCode(500) : Ok(OrderViewModel.ToModel(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occurred in Order Controller, create order");
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates Order status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatusPatch">Body, containing new order Status</param>
        /// <returns></returns>
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId, [FromBody] OrderStatusPatch orderStatusPatch)
        {
            try
            {
                var result = await _orderService.ChangeOrderStatus(orderId, orderStatusPatch.OrderStatus);

                return result ? Ok() : StatusCode(500);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occurred in Order Controller, create order");
                return BadRequest();
            }
        }
    }
}