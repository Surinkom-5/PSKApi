using FlowerShop.Core.Constants;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;
using FlowerShop.Web.Api;
using FlowerShop.Web.Extensions;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(ILogger<ShoppingCartController> logger,
            IShoppingCartRepository shoppingCartRepository,
            IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// Gets shopping cart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                // If user is authenticated
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var cart = await _shoppingCartService.GetUserCart(User.Identity.GetUserId());

                    return cart is null ? NotFound() : Ok(CartViewModel.ToModel(cart));
                }

                // If user has "cartCookie" header value
                if (Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues))
                {
                    var cartCookie = headerValues.FirstOrDefault();
                    var cart = await _shoppingCartRepository.GetCartWithItemsByPublicIdAsync(cartCookie);

                    return cart is null ? NotFound() : Ok(CartViewModel.ToModel(cart));
                }
                else
                {
                    // Means user does not have a cart yet
                    var cart = await _shoppingCartService.CreateCart();

                    if (cart is null) return BadRequest();

                    Response.Headers.Add(CookieConstants.CartCookie, cart.Id.ToString());
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
        /// <param name="cartItemPatchModel"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemPatchModel cartItemPatchModel)
        {
            try
            {
                // If user has "cartCookie" header value
                if (!Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues)) return BadRequest();
                if (cartItemPatchModel == null) return BadRequest();

                var cartCookie = headerValues.FirstOrDefault();
                var result = await _shoppingCartService.AddItemToCart(cartCookie, cartItemPatchModel.ProductId,
                    cartItemPatchModel.Quantity);

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
                if (!Request.Headers.TryGetValue(CookieConstants.CartCookie, out StringValues headerValues)) return BadRequest();

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