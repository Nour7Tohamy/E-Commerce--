using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.DTOs.ProductDtos;
using E_Commerce.Models;

namespace E_Commerce.Service.Interfaces
{
    public interface ICategoryService
    {
        // Get Methods
        Task<OperationResultGeneric<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<OperationResultGeneric<CategoryDto>> GetCategoryByIdAsync(int id);
        Task<OperationResultGeneric<IEnumerable<CategoryWithProductsDto>>> GetCategoriesWithProductsAsync();
        Task<OperationResultGeneric<IEnumerable<CategoryWithCountDto>>> GetCategoriesWithProductCountAsync();
        Task<OperationResultGeneric<IEnumerable<CategorySalesDto>>> GetPopularCategoriesAsync();
        Task<OperationResultGeneric<IEnumerable<CategoryDto>>> SearchCategoryAsync(string name);

        // Create Methods
        Task<OperationResultGeneric<CategoryDto>> AddCategoryAsync(CreateCategoryDto dto);
        Task<OperationResultGeneric<CategoryWithProductsDto>> AddCategoryWithProductsAsync(CreateCategoryWithProductsDto dto);

        // Update Methods
        Task<OperationResultGeneric<CategoryWithProductsDto>> UpdateCategoryWithProductsAsync(int id, UpdateCategoryWithProductsDto dto);

        // Delete Methods
        Task<OperationResult> DeleteCategoryAsync(int id);
    }
}
