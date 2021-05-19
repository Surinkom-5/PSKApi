using FlowerShop.Core.Entities;
using FlowerShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Infrastructure.Services
{
    public interface ICartItemService
    {
        public Task ReduceProductAvailabilityAsync(List<CartItem> cartItems);
    }

    public class CartItemService : ICartItemService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(
            AppDbContext context,
            ILogger<CartItemService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task ReduceProductAvailabilityAsync(List<CartItem> cartItems)
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
    }
}