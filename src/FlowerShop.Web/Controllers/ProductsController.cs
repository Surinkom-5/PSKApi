using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Web.Api;
using FlowerShop.Web.Models;
using FlowerShop.Web.Patch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        /// Gets category types which have a product
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/Categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _productRepository.GetAvailableProductTypesAsync();

                return categories.Any() ? Ok(CategoryViewModel.ToModel(categories)) : NotFound();
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
        /// <param name="createProductModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductModel createProductModel)
        {
            try
            {
                await _productService.CreateProductAsync(createProductModel.Name, createProductModel.Price,
                    createProductModel.Description, createProductModel.ProductType);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured in Product Controller, CreateProductAsync");
                return BadRequest();
            }
        }

        /// <summary>
        /// Update product details
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{productId}")]
        public async Task<IActionResult> PatchProductDetails([FromRoute] int productId, [FromBody] ProductPatch productPatch)
        {
            try
            {
                var success = await _productService.UpdateProductAsync(productId, productPatch.Name, productPatch.Price, productPatch.Description, productPatch.Quantity);

                return success ? Ok() : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Product Controller, when updating product details");
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> PathUserAddress([FromRoute] int productId)
        {
            try
            {
                var success = await _productService.RemoveProductAsync(productId);

                return success ? Ok() : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Product Controller, when deleting product");
                return BadRequest();
            }
        }
    }
}