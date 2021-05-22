using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public class AnonymousOrderCreatorStrategy : IOrderCreatorStrategy
    {
        private readonly AppDbContext _dbContext;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<AnonymousOrderCreatorStrategy> _logger;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ICartItemService _cartItemService;

        public AnonymousOrderCreatorStrategy(
            AppDbContext dbContext,
            IShoppingCartRepository shoppingCartRepository,
            ILogger<AnonymousOrderCreatorStrategy> logger,
            IUserService userService,
            IUserRepository userRepository,
            ICartItemService cartItemService)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _userService = userService;
            _userRepository = userRepository;
            _cartItemService = cartItemService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartWithItemsByPublicIdAsync(orderModel.CartId.ToString());
                if (cart is null)
                {
                    throw new ArgumentNullException(nameof(cart));
                }
                if (cart.CartItems == null || cart.CartItems.Count <= 0)
                {
                    throw new ArgumentException("Cart items must can't be empty or null");
                }

                await _cartItemService.ReduceProductAvailabilityAsync(cart.CartItems.ToList());

                var userId = await _userRepository.CheckIfUserExists(orderModel.Email)
                    ? (await _userRepository.GetUserByEmailAsync(orderModel.Email)).UserId
                    : await _userService.AddUserAsync("unregistered user", orderModel.Email);

                var user = await _userRepository.GetUserByIdAsync(userId);
                user.SetPhoneNumber(orderModel.PhoneNumber);
                var address = new Address(userId, orderModel.Address, orderModel.City, orderModel.PostCode);
                user.AddAddress(address);
                await _dbContext.SaveChangesAsync();

                var order = new Order(userId, address.AddressId, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    throw new ArgumentNullException(nameof(createdOrder.Entity));
                }

                await _dbContext.SaveChangesAsync();

                await UpdateOrderItemsFromCart(order.OrderId, cart);

                await transaction.CommitAsync();
                return createdOrder.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: CreateOrder");
                await transaction.RollbackAsync();
                return null;
            }
        }

        private async Task UpdateOrderItemsFromCart(int orderId, Cart cart)
        {
            try
            {
                var orderItems = CartItem.ToOrderItems(cart.CartItems.ToList(), orderId);
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                _dbContext.CartItems.RemoveRange(cart.CartItems.ToList());
                cart.SetPrice(0);
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: UpdateOrderItemsFromCart");
                throw;
            }
        }
    }
}