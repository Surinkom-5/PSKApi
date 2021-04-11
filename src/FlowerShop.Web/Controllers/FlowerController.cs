using FlowerShop.Infrastructure;
using FlowerShop.Web.Api;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    public class FlowerController : BaseApiController
    {
        private readonly IFlowerRepository _flowerRepository;

        public FlowerController(IFlowerRepository flowerRepository)
        {
            _flowerRepository = flowerRepository;
        }

        [HttpGet("{flowerId}")]
        public async Task<IActionResult> GetFlower([FromRoute] int flowerId)
        {
            var flower = await _flowerRepository.GetFlowerById(flowerId);

            return Ok(flower);
        }
    }
}
