using E_Commerce.Repository._Generics;

namespace E_Commerce.Repository.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();
        Task<Category> GetCategoryWithProductsByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name);
        Task SaveChangesAsync(); 
    }
}