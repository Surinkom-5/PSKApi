using System;
using System.Threading.Tasks;
using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlowerShop.Web.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(ILogger<ProductsController> logger, IShoppingCartRepository shoppingCartRepository)
        {
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
        }

        /// <summary>
        /// Gets shopping cart by provided Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync([FromRoute] int id)
        {
            try
            {
                var cart = await _shoppingCartRepository.GetCartByIdAsync(id);

                return cart is null ? NotFound() : Ok(CartViewModel.ToModel(cart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Shopping Cart Controller, GET shopping cart by id");
                return BadRequest();
            }
        }
    }
}
