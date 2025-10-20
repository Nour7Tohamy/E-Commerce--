using E_Commerce.DTOs.ProductDtos;
using E_Commerce.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetByCategoryAsync(categoryId); 
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var products = await _productService.SearchAsync(name); 
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.AddAsync(dto); 

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateAsync(dto); 

            if (!result.Success) 
                return NotFound(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id); 

            if (!result.Success) 
                return NotFound(result.Message);

            return Ok(result.Message);
        }
    }
}