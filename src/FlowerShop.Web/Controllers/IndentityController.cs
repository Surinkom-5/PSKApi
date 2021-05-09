using FlowerShop.Infrastructure.Services;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    public class IdentityController : BaseApiController
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IIdentityService _identityService;

        public IdentityController(
            ILogger<OrdersController> logger,
            IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel userRegistrationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultViewModel()
                {
                    Errors = new List<string>() { "Invalid model" }
                });
            }

            var response = await _identityService.RegisterAsync(userRegistrationModel.Email,
                userRegistrationModel.Name, userRegistrationModel.Password);

            if (string.IsNullOrEmpty(response.Token))
            {
                return BadRequest(new AuthResultViewModel()
                {
                    Errors = response.Errors
                });
            }

            return Ok(new AuthResultViewModel()
            {
                Token = response.Token
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultViewModel()
                {
                    Errors = new List<string>() { "Invalid model" }
                });
            }
            var (token, error) = await _identityService.LoginAsync(userLoginModel.Email, userLoginModel.Password);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new AuthResultViewModel()
                {
                    Errors = new List<string>() { error }
                });
            }

            return Ok(new AuthResultViewModel()
            {
                Token = token
            });
        }
    }
}