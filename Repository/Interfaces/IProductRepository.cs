using E_Commerce.Models;
using E_Commerce.Repository._Generics;

namespace E_Commerce.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product?> GetProductByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string name);
        Task SaveChangesAsync();
    }
}