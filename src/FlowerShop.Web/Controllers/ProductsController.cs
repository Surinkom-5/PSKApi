using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlowerShop.Web.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository,
            ILogger<ProductsController> logger,
            IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Gets product by provided product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductAsync([FromRoute] int productId)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(productId);

                return product is null ? NotFound() : Ok(ProductViewModel.ToModel(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, get product");
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets all filtered products if no filtering parameters are given returns all products
        /// </summary>
        /// <param name="productFiltersModel">Query filters</param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductFiltersModel productFiltersModel)
        {
            try
            {
                var product = await _productRepository.GetProductsByFiltersAsync(productFiltersModel.SearchText,
                    productFiltersModel.ProductType);

                return product is null ? NotFound() : Ok(ProductViewModel.ToModel(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, GetProductsAsync");
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="createProductViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductModel createProductViewModel)
        {
            try
            {
                await _productService.CreateProductAsync(createProductViewModel.Name, createProductViewModel.Price,
                    createProductViewModel.Description, createProductViewModel.ProductType);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, CreateProductAsync");
                return BadRequest();
            }
        }
    }
}
