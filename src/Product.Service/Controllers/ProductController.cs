using common.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Service.Features.Products.Commands;
using Product.Service.Features.Products.Dtos;
using Product.Service.Features.Products.Queries;


namespace Product.Service.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const string AdminRole = "Admin";

        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        /// <summary>
        /// Get all product endpoint
        /// </summary>
        /// <returns></returns>
                       //[AllowAnonymous]
        [HttpGet("products")]
        [Authorize(Policies.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsQuery());
            return Ok(new
            {
                status_code = 200,
                products
            });
        }

        /// <summary>
        /// Get product by product Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("products/{id}")]
        [Authorize(Policies.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { error = "Invalid product ID" });
            }

            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            return Ok(new
            {
                status_code = 200,
                product
            });
        }

        /// <summary>
        /// Add Product - add a product 
        /// </summary>
        [HttpPost("products/add-products")]
        [Authorize(Policies.Write)]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreationDto body)
        {
            var command = new AddProductsCommand(body);
            var response = await _mediator.Send(command);

            var successResponse = new SuccessResponseDto<ProductDto>();
            successResponse.Data = response;
            successResponse.Message = "Product added Successfully";
            return StatusCode(StatusCodes.Status201Created, successResponse);
        }

        /// <summary>
        /// Product Deletion - deletes a product 
        /// </summary>
        [HttpDelete("products/{id:guid}")]
        [Authorize(Policies.Write)]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            try
            {
                var command = new DeleteProductByIdCommand(id);
                var response = await _mediator.Send(command);
                return response != null
                    ? Ok(new SuccessResponseDto<object> { Message = "Product deleted successfully", Data = true })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Product with ID {id} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        /// <summary>
        /// Edit user products with update timestamp
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        [HttpPut("products/{id:guid}")]
        [Authorize(Policies.Write)]
        [ProducesResponseType(typeof(SuccessResponseDto<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                var command = new UpdateProductCommand(id, updateProductDto);
                var response = await _mediator.Send(command);
                return response != null
                    ? Ok(new SuccessResponseDto<ProductDto> { Message = "Product updated successfully", Data = response })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Product with ID {id} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }
    }
}
