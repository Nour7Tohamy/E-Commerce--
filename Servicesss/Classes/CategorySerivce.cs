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

        public async Task<OperationResultGeneric<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var result = categories.Select(c => new CategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name
            }).ToList();

            return OperationResultGeneric<IEnumerable<CategoryDto>>.Ok(result, "Categories retrieved successfully.");
        }

        public async Task<OperationResultGeneric<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return OperationResultGeneric<CategoryDto>.Fail($"Category with ID {id} not found.");

            var dto = new CategoryDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name
            };
            return OperationResultGeneric<CategoryDto>.Ok(dto, "Category retrieved successfully.");
        }

        public async Task<OperationResultGeneric<IEnumerable<CategoryWithProductsDto>>> GetCategoriesWithProductsAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            var result = categories.Select(c => new CategoryWithProductsDto
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

            return OperationResultGeneric<IEnumerable<CategoryWithProductsDto>>.Ok(result, "Categories with products retrieved successfully.");
        }

        public async Task<OperationResultGeneric<IEnumerable<CategoryWithCountDto>>> GetCategoriesWithProductCountAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            var result = categories.Select(c => new CategoryWithCountDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();

            return OperationResultGeneric<IEnumerable<CategoryWithCountDto>>.Ok(result, "Categories with product count retrieved successfully.");
        }

        public async Task<OperationResultGeneric<IEnumerable<CategorySalesDto>>> GetPopularCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithProductsAsync();

            var result = categories
                .Select(c => new CategorySalesDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    TotalSales = c.Products?.Sum(p => p.Price) ?? 0
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(5)
                .ToList();

            return OperationResultGeneric<IEnumerable<CategorySalesDto>>.Ok(result, "Top categories retrieved successfully.");
        }

        public async Task<OperationResultGeneric<IEnumerable<CategoryDto>>> SearchCategoryAsync(string name)
        {
            var categories = await _categoryRepository.SearchCategoryAsync(name);

            var result = categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToList();

            return OperationResultGeneric<IEnumerable<CategoryDto>>.Ok(result, "Search completed successfully.");
        }

        #endregion

        #region Create Methods

        public async Task<OperationResultGeneric<CategoryDto>> AddCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.CategoryName
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            var dtoResult = new CategoryDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name
            };

            return OperationResultGeneric<CategoryDto>.Ok(dtoResult, $"Category '{dto.CategoryName}' created successfully.");
        }

        public async Task<OperationResultGeneric<CategoryWithProductsDto>> AddCategoryWithProductsAsync(CreateCategoryWithProductsDto dto)
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

            var dtoResult = new CategoryWithProductsDto
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

            return OperationResultGeneric<CategoryWithProductsDto>.Ok(dtoResult, $"Category '{dto.CategoryName}' created successfully.");
        }

        #endregion

        #region Update Methods

        public async Task<OperationResultGeneric<CategoryWithProductsDto>> UpdateCategoryWithProductsAsync(int id, UpdateCategoryWithProductsDto dto)
        {
            var category = await _categoryRepository.GetCategoryWithProductsByIdAsync(id);

            if (category == null)
                return OperationResultGeneric<CategoryWithProductsDto>.Fail($"Category with ID {id} not found.");

            category.Name = dto.CategoryName;

            if (dto.Products != null && dto.Products.Any())
            {
                category.Products = dto.Products.Select(p => new Product
                {
                    Name = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = category.Id
                }).ToList();
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            var dtoResult = new CategoryWithProductsDto
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

            return OperationResultGeneric<CategoryWithProductsDto>.Ok(dtoResult, $"Category '{dto.CategoryName}' updated successfully.");
        }

        #endregion

        #region Delete Methods

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
