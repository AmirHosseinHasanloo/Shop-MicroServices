using System.Net;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        #region Dependency Injection
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository
            , ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        #endregion

        #region Get Products

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var Products = await _productRepository.GetProducts();

            return Ok(Products);
        }
        #endregion

        #region Get Product By Id

        [HttpGet("{id:length(24)}", Name = "GetProductById")]
        [ProducesResponseType(((int)HttpStatusCode.NotFound))]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var Product = await _productRepository.GetProductById(id);

            if (Product == null)
            {
                _logger.LogError($"Product with id: {id} is not exist");
                return NotFound();
            }

            return Ok(Product);
        }
        #endregion

        #region Get Products By Category Name

        [HttpGet("[action]/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var Products = await _productRepository.GetProductsByCategory(category);

            return Ok(Products);
        }
        #endregion

        #region Create Product

        [HttpPost]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        [ProducesResponseType(((int)HttpStatusCode.BadRequest))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.CreateProduct(product);
                return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
            }
            return BadRequest();
        }
        #endregion

        #region Update Product

        [HttpPut]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            await _productRepository.UpdateProduct(product);
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        #endregion
    }
}
