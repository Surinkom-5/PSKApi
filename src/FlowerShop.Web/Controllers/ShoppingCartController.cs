using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Infrastructure.Services.Interfaces;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using FlowerShop.Web.Patch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace FlowerShop.Web.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartService _shoppingCartService;


        public ShoppingCartController(ILogger<ProductsController> logger, IShoppingCartRepository shoppingCartRepository, IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// Gets shopping cart by provided Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                // If user has "cartCookie" header value
                if (Request.Headers.TryGetValue("cartCookie", out StringValues headerValues))
                {
                    var cartCookie = headerValues.FirstOrDefault();
                    var cart = await _shoppingCartRepository.GetCartByPublicIdAsync(cartCookie);

                    return cart is null ? NotFound() : Ok(CartViewModel.ToModel(cart));
                }
                else
                {
                    // Means user does not have a cart yet
                    var cart = await _shoppingCartService.CreateCart();

                    if (cart is null) return StatusCode(500);

                    Response.Cookies.Append("cartCookie", cart.PublicId.ToString());
                    return Ok(CartViewModel.ToModel(cart));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, GET shopping cart");
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds specified item to active cart
        /// Request must contain cartCookie
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemPatch body)
        {
            try
            {
                // If user has "cartCookie" header value
                if (!Request.Headers.TryGetValue("cartCookie", out StringValues headerValues)) return BadRequest();
                if (body == null) return BadRequest();

                
                var cartCookie = headerValues.FirstOrDefault();
                var result = await _shoppingCartService.AddItemToCart(cartCookie, body.productId, body.quantity);

                return result is false ? NotFound() : Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, Add shopping cart item");
                return BadRequest();
            }
        }

        /// <summary>
        /// Removes item from active cart
        /// Request must contain cartCookie
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] int itemId)
        {
            try
            {
                // If user has "cartCookie" header value
                if (!Request.Headers.TryGetValue("cartCookie", out StringValues headerValues)) return BadRequest();

                var cartCookie = headerValues.FirstOrDefault();
                var result = await _shoppingCartService.RemoveItemFromCart(cartCookie, itemId);

                return result is false ? NotFound() : Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occurred in Shopping Cart Controller, Remove shopping cart item");
                return BadRequest();
            }
        }
    }
}
