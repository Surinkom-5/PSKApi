using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using FlowerShop.Infrastructure.CustomModels;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlowerShop.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;

        public OrderService(AppDbContext dbContext, IOrderRepository orderRepository,
            IShoppingCartRepository shoppingCartRepository, ILogger<OrderService> logger, IUserService userService,
            IAddressService addressService, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _dbContext = dbContext;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _userService = userService;
            _addressService = addressService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Order> CreateOrder(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(orderModel.CartId.ToString());
                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == orderModel.CartId).ToListAsync();

                if (cartItems == null || cartItems.Count <= 0 || cart == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await ReduceProductAvailability(_dbContext, cartItems);

                int userId;
                if (await _userRepository.CheckIfUserExists(orderModel.Email))
                {
                    // Means such a user already exists
                    var userTemp = await _userRepository.GetUserByEmailAsync(orderModel.Email);
                    userId = userTemp.UserId;
                }
                else
                {
                    // Create user
                    userId = await _userService.AddUserAsync("unregistered user", orderModel.Email);
                }

                var user = await _userRepository.GetUserByIdAsync(userId);
                user.SetPhoneNumber(orderModel.PhoneNumber);
                await _dbContext.SaveChangesAsync();

                var addressId = await _addressService.AddNewAddressAsync(userId, orderModel.Address, orderModel.City,
                    orderModel.PostCode);
                    
                var order = new Order(userId, addressId, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    await transaction.RollbackAsync();
                    return null;
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

        private async Task ReduceProductAvailability(AppDbContext dbContext, List<CartItem> cartItems)
        {
            try
            {
                var idsToReduce = cartItems.Select(i => i.ProductId).ToList();
                var productsToReduce =
                    await dbContext.Products.Where(i => idsToReduce.Contains(i.ProductId)).ToListAsync();

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

                dbContext.Products.UpdateRange(productsToReduce);
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

        public async Task<Order> CreateOrderForAuthenticatedUser(CreateOrderModel orderModel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Get cart by user Id
                var cart = await _shoppingCartRepository.GetCartByUserId((int) orderModel.UserId);
                var cartItems = await _dbContext.CartItems.Where(c => c.CartId == cart.Id).ToListAsync();

                if (cartItems == null || cartItems.Count <= 0 || cart == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await ReduceProductAvailability(_dbContext, cartItems);

                var order = new Order((int)orderModel.UserId, (int)orderModel.AddressId, orderModel.Comment, cart.Price);
                var createdOrder = await _dbContext.Orders.AddAsync(order);

                if (createdOrder.Entity == null)
                {
                    await transaction.RollbackAsync();
                    return null;
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