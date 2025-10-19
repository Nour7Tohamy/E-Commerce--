using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.Models;

namespace E_Commerce.Repository.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();
        Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCountAsync();
        Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name);
        Task<IEnumerable<CategorySalesDto>> GetPopularCategoriesAsync();
        Task<Category> GetCategoryWithProductsByIdAsync(int id);
    }
}