[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound($"Category with ID {id} not found");

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = await _categoryService.AddCategoryAsync(dto); 
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("with-products")]
    public async Task<IActionResult> AddWithProducts(CreateCategoryWithProductsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = await _categoryService.AddCategoryWithProductsAsync(dto); 
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryWithProductsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = await _categoryService.UpdateCategoryWithProductsAsync(id, dto); 
            return Ok(category);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id); 

        if (!result.Success)
            return NotFound(result.Message);

        return Ok(result.Message);
    }
}