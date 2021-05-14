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
            var response = await _identityService.RegisterAsync(userRegistrationModel.Email,
                userRegistrationModel.Name, userRegistrationModel.Password);

            return string.IsNullOrEmpty(response.Token)
                ? BadRequest(new AuthResultViewModel() { Errors = response.Errors })
                : Ok(new AuthResultViewModel() { Token = response.Token });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel userLoginModel)
        {
            var (token, error) = await _identityService.LoginAsync(userLoginModel.Email, userLoginModel.Password);

            return string.IsNullOrEmpty(token)
                ? BadRequest(new AuthResultViewModel() { Errors = new List<string>() { error } })
                : Ok(new AuthResultViewModel() { Token = token });
        }
    }
}