using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.DTOs.ProductDtos;
using E_Commerce.Models;
using E_Commerce.Repository.Interfaces;
using E_Commerce.Service.Interfaces;

namespace E_Commerce.Service.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region Get Methods

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name
            }).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name
            };
        }

        public async Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            return categories.Select(c => new CategoryWithProductsDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Products = c.Products?.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList() ?? new List<ProductDto>()
            }).ToList();
        }

        public async Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCountAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            return categories.Select(c => new CategoryWithCountDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();
        }

        public async Task<IEnumerable<CategorySalesDto>> GetPopularCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            return categories
                .Select(c => new CategorySalesDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    TotalSales = c.Products?.Sum(p => p.Price) ?? 0
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(5)
                .ToList();
        }

        public async Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name)
        {
            var categories = await _categoryRepository.SearchCategoryAsync(name);

            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToList();
        }

        #endregion

        #region Create Methods

        // ✅ شلنا OperationResult - بنرجع CategoryDto مباشرة
        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.CategoryName
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name
            };
        }

        // ✅ شلنا OperationResult - بنرجع CategoryWithProductsDto مباشرة
        public async Task<CategoryWithProductsDto> AddCategoryWithProductsAsync(CreateCategoryWithProductsDto dto)
        {
            var category = new Category
            {
                Name = dto.CategoryName,
                Products = dto.Products?.Select(p => new Product
                {
                    Name = p.ProductName,
                    Description = p.Description,
                    Price = p.Price
                }).ToList() ?? new List<Product>()
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryWithProductsDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Products = category.Products.Select(p => new ProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = category.Id,
                    CategoryName = category.Name
                }).ToList()
            };
        }

        #endregion

        #region Update Methods

        // ✅ شلنا OperationResult - بنرجع CategoryWithProductsDto مباشرة أو Exception
        public async Task<CategoryWithProductsDto> UpdateCategoryWithProductsAsync(int id, UpdateCategoryWithProductsDto dto)
        {
            var category = await _categoryRepository.GetCategoryWithProductsByIdAsync(id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            category.Name = dto.CategoryName;

            if (dto.Products != null)
            {
                category.Products.Clear();
                foreach (var productDto in dto.Products)
                {
                    var product = new Product
                    {
                        Name = productDto.ProductName,
                        Description = productDto.Description,
                        Price = productDto.Price,
                        CategoryId = category.Id
                    };
                    category.Products.Add(product);
                }
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryWithProductsDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Products = category.Products.Select(p => new ProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = category.Id,
                    CategoryName = category.Name
                }).ToList()
            };
        }

        #endregion

        #region Delete Methods

        // ✅ ده سيبناه OperationResult
        public async Task<OperationResult> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return OperationResult.Fail($"Category with ID {id} not found.");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return OperationResult.Ok($"Category '{category.Name}' deleted successfully.");
        }

        #endregion
    }
}