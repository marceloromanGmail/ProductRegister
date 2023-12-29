using Application.Main.Products.Commands.CreateProduct;
using Application.Main.Products.Dto;
using Application.Main.Products.Queries.GetProductById;
using Application.Main.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Get a list of products.
        /// </summary>
        /// <param name="name">Filter products by name.</param>
        /// <returns>List of products.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get(string name = null)
        {
            var result = await _mediator.Send(new GetProductsQuery(name));
            return Ok(result);
        }

        /// <summary>
        /// Get a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Details of the product.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailProductDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="param">The parameters for creating a product.</param>
        /// <returns>Details of the created product.</returns>
        [HttpPost]
        public async Task<ActionResult<CreatedProductDto>> Post(CreateProductCommand param)
        {
            var result = await _mediator.Send(param);
            return Ok(result);
        }

        /// <summary>
        /// Update a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="param">The parameters for updating the product.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateProductCommand param)
        {
            param.Id = id;
            await _mediator.Send(param);
            return Ok();
        }

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok();
        }
    }
}
