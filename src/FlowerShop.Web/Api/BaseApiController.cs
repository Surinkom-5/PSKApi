using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : Controller
    {
    }
}