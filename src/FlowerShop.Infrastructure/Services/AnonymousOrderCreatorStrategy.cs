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
        private readonly IAddressService _addressService;
        private readonly IUserRepository _userRepository;

        public AnonymousOrderCreatorStrategy(
            AppDbContext dbContext,
            IShoppingCartRepository shoppingCartRepository,
            ILogger<AnonymousOrderCreatorStrategy> logger,
            IUserService userService,
            IAddressService addressService,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _userService = userService;
            _addressService = addressService;
            _userRepository = userRepository;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(orderModel.CartId.ToString());
                if (cart is null)
                {
                    throw new ArgumentNullException(nameof(cart));
                }

                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == orderModel.CartId).ToListAsync();
                if (cartItems == null || cartItems.Count <= 0)
                {
                    throw new ArgumentException("Cart items must can't be empty or null");
                }

                await ReduceProductAvailability(cartItems);

                var userId = await _userRepository.CheckIfUserExists(orderModel.Email)
                    ? (await _userRepository.GetUserByEmailAsync(orderModel.Email)).UserId
                    : await _userService.AddUserAsync("unregistered user", orderModel.Email);

                var user = await _userRepository.GetUserByIdAsync(userId);
                user.SetPhoneNumber(orderModel.PhoneNumber);
                await _dbContext.SaveChangesAsync();

                var addressId = await _addressService.AddNewAddressAsync(userId, orderModel.Address, orderModel.City,
                    orderModel.PostCode);
                await _dbContext.SaveChangesAsync();

                var order = new Order(userId, addressId, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    throw new ArgumentNullException(nameof(createdOrder.Entity));
                }

                await _dbContext.SaveChangesAsync();

                await UpdateOrderItemsFromCart(_dbContext, cartItems, order.OrderId, cart);

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

        private async Task ReduceProductAvailability(List<CartItem> cartItems)
        {
            try
            {
                var idsToReduce = cartItems.Select(i => i.ProductId).ToList();
                var productsToReduce =
                    await _dbContext.Products.Where(i => idsToReduce.Contains(i.ProductId)).ToListAsync();

                productsToReduce.ForEach(p =>
                {
                    var reducedAvailability =
                        p.AvailabilityCount - cartItems.First(i => i.ProductId == p.ProductId).Quantity;
                    if (reducedAvailability < 0)
                    {
                        throw new InvalidOperationException(
                            $"Attempting to buy more of Product with id {p.ProductId} than is currently available.\n");
                    }

                    p.SetAvailabilityCount(reducedAvailability);
                });

                _dbContext.Products.UpdateRange(productsToReduce);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: ReduceProductAvailability");
                throw;
            }
        }

        private async Task UpdateOrderItemsFromCart(AppDbContext dbContext, List<CartItem> cartItems, int orderId, Cart cart)
        {
            try
            {
                var orderItems = CartItem.ToOrderItems(cartItems, orderId);
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                dbContext.CartItems.RemoveRange(cartItems);
                cart.SetPrice(0);
                dbContext.Carts.Update(cart);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in OrderService: UpdateOrderItemsFromCart");
                throw;
            }
        }
    }
}