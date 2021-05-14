using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Web.Api;
using FlowerShop.Web.Extensions;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AddressController : BaseApiController
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IAddressService _addressService;
        private readonly ILogger<OrdersController> _logger;

        public AddressController(
            ILogger<OrdersController> logger,
            IAddressRepository addressRepository,
            IAddressService addressService)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _addressService = addressService;
        }

        /// <summary>
        /// Gets user addresses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserAddresses()
        {
            try
            {
                var addresses = await _addressRepository.GetUserAddressesAsync(User.Identity.GetUserId());

                return Ok(AddressViewModel.ToModel(addresses));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, get address by address id");
                return BadRequest();
            }
        }

        /// <summary>
        /// Add new address to user
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewAddress([FromBody] CreateAddressModel addressModel)
        {
            try
            {
                await _addressService.AddNewAddressAsync(User.Identity.GetUserId(), addressModel.Street,
                    addressModel.City, addressModel.PostalCode);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when adding address");
                return BadRequest();
            }
        }

        /// <summary>
        /// Update address
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{addressId}")]
        public async Task<IActionResult> PathUserAddress([FromRoute] int addressId, [FromBody] CreateAddressModel addressModel)
        {
            try
            {
                var success = await _addressService.UpdateAddressAsync(User.Identity.GetUserId(), addressId,
                    addressModel.Street, addressModel.City, addressModel.PostalCode);

                return success ? Ok() : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when updating address");
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete user address
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> PathUserAddress([FromRoute] int addressId)
        {
            try
            {
                var success = await _addressService.RemoveAddressAsync(User.Identity.GetUserId(), addressId);

                return success ? Ok() : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Address Controller, when deleting address");
                return BadRequest();
            }
        }
    }
}