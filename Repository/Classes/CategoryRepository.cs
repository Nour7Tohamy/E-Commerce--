using E_Commerce.Data;
using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.DTOs.ProductDtos;
using E_Commerce.Models;
using E_Commerce.Repository._Generics;
using E_Commerce.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository.Classes
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly EcommerceDbContext _context;

        public CategoryRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .Select(c => new CategoryWithProductsDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    Products = c.Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        CategoryId = c.Id,
                        CategoryName = c.Name
                    }).ToList()
                })
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCountAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryWithCountDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    ProductCount = c.Products.Count()
                })
                .ToListAsync();
        }

        public virtual async Task<Category> GetCategoryWithProductsByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public virtual async Task<IEnumerable<CategorySalesDto>> GetPopularCategoriesAsync()
        {
            return await _context.CartItems
                .GroupBy(ci => new { ci.Product.Category.Id, ci.Product.Category.Name })
                .Select(g => new CategorySalesDto
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    TotalSales = g.Sum(ci => ci.Quantity)
                })
                .OrderByDescending(c => c.TotalSales)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(name))
                .Select(c => new CategoryDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name
                })
                .ToListAsync();
        }

        Task ICategoryRepository.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}