using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        #region GET Endpoints

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();

            if (categories == null || !categories.Any())
                return NotFound("No categories found");

            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound($"Category with ID {id} not found");

            return Ok(category);
        }

        [HttpGet("GetCategoriesWithProducts")]
        public async Task<ActionResult<IEnumerable<CategoryWithProductsDto>>> GetCategoriesWithProducts()
        {
            var categories = await _service.GetCategoriesWithProductsAsync();

            if (categories == null || !categories.Any())
                return NotFound("No categories with products found");

            return Ok(categories);
        }

        [HttpGet("GetCategoriesWithProductCount")]
        public async Task<ActionResult<IEnumerable<CategoryWithCountDto>>> GetCategoriesWithProductCount()
        {
            var categories = await _service.GetCategoriesWithProductCountAsync();

            if (categories == null || !categories.Any())
                return NotFound("No categories found");

            return Ok(categories);
        }

        [HttpGet("GetPopularCategories")]
        public async Task<ActionResult<IEnumerable<CategorySalesDto>>> GetPopularCategories()
        {
            var categories = await _service.GetPopularCategoriesAsync();

            if (categories == null || !categories.Any())
                return NotFound("No popular categories found");

            return Ok(categories);
        }

        [HttpGet("SearchCategory")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> SearchCategory([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search name cannot be empty");

            var categories = await _service.SearchCategoryAsync(name);

            if (categories == null || !categories.Any())
                return NotFound($"No categories found matching '{name}'");

            return Ok(categories);
        }

        #endregion

        #region POST Endpoints

        [HttpPost("AddCategory")]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _service.AddCategoryAsync(dto);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.CategoryId },
                category
            );
        }

        [HttpPost("AddCategoryWithProducts")]
        public async Task<ActionResult<CategoryWithProductsDto>> AddCategoryWithProducts([FromBody] CreateCategoryWithProductsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _service.AddCategoryWithProductsAsync(dto);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.CategoryId },
                category
            );
        }

        #endregion

        #region PUT Endpoints

        [HttpPut("UpdateCategoryWithProducts/{id}")]
        public async Task<ActionResult<CategoryWithProductsDto>> UpdateCategoryWithProducts(int id, [FromBody] UpdateCategoryWithProductsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _service.UpdateCategoryWithProductsAsync(id, dto);

            if (category == null)
                return NotFound($"Category with ID {id} not found");

            return Ok(category);
        }

        #endregion

        #region DELETE Endpoints

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _service.DeleteCategoryAsync(id);
            return Ok(new { Message = $"Category with ID {id} deleted successfully" });
        }

        #endregion
    }
}