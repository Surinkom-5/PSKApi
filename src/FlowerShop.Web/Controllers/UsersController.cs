using FlowerShop.Infrastructure;
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
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ILogger<UserController> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets user addresses
        /// </summary>
        /// <returns></returns>
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(User.Identity.GetUserId());

                return Ok(UserViewModel.ToModel(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in user Controller, get user");
                return BadRequest();
            }
        }
    }
}