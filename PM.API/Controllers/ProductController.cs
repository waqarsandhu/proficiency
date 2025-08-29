using Microsoft.AspNetCore.Mvc;
using PM.Common.Common;
using PM.Common.Dto;
using PM.Common.Interfaces;

namespace PM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetProductInputDto input)
        {
            var products = await _service.GetAllAsync(input);
            return Ok(products);
        }

        // GET: api/product/5
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id <= 0)
            {
                return Problem(
                    detail: Constants.ProductIDErrorMessage,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: Constants.BadRequestTitle
                );
            }

            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return Problem(
                    detail: string.Format(Constants.ProductIDNotFoundMessage, id),
                    statusCode: StatusCodes.Status404NotFound,
                    title: Constants.NotFoundTitle
                );
            }

            return Ok(product);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var createdProduct = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/product/5
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateOrUpdateProductDto dto)
        {
            if (id <= 0)
            {
                return Problem(
                    detail: Constants.ProductIDErrorMessage,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: Constants.BadRequestTitle
                );
            }

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return Problem(
                    detail: string.Format(Constants.ProductIDNotFoundMessage, id),
                    statusCode: StatusCodes.Status404NotFound,
                    title: Constants.NotFoundTitle
                );

            return Ok(new { Message = Constants.UpdatedMessage });
        }


        // DELETE: api/product/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id <= 0)
            {
                return Problem(
                    detail: Constants.ProductIDErrorMessage,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: Constants.BadRequestTitle
                );
            }

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return Problem(
                    detail: string.Format(Constants.ProductIDNotFoundMessage, id),
                    statusCode: StatusCodes.Status404NotFound,
                    title: Constants.NotFoundTitle
                );

            return NoContent();
        }
    }
}
