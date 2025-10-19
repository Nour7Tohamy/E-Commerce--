using E_Commerce.DTOs.CategoryDtos;

namespace E_Commerce.Service.Interfaces
{
    public interface ICategoryService
    {
        // Get Methods
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();
        Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCountAsync();
        Task<IEnumerable<CategorySalesDto>> GetPopularCategoriesAsync();
        Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name);

        // Create Methods
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto);
        Task<CategoryWithProductsDto> AddCategoryWithProductsAsync(CreateCategoryWithProductsDto dto);

        // Update Methods
        Task<CategoryWithProductsDto> UpdateCategoryWithProductsAsync(int id, UpdateCategoryWithProductsDto dto);

        // Delete Methods
        Task DeleteCategoryAsync(int id);
    }
}