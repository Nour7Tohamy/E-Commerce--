using E_Commerce.Common;

namespace E_Commerce.Service.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        #region Get Methods

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();

            // Manual Mapping
            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "No Category"
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdWithCategoryAsync(id);

            if (product == null)
                return null;

            // Manual Mapping
            return new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "No Category"
            };
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId) 
        {
            var products = await _productRepository.GetAllProductsByCategoryIdAsync(categoryId);

            // Manual Mapping
            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "No Category"
            }).ToList();
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string name) 
        {
            var products = await _productRepository.SearchProductsAsync(name);

            // Manual Mapping
            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "No Category"
            }).ToList();
        }

        #endregion

        #region Create Method

        public async Task<OperationResultGeneric<ProductDto>> AddAsync(CreateProductDto dto)
        {
            // Validation
            var categoryExists = await _categoryRepository.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                return OperationResultGeneric<ProductDto>.Fail($"Category with ID {dto.CategoryId} not found.");

            // Manual Mapping
            var product = new Product
            {
                Name = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            var productdto = new ProductDto()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Not Exist Category",
            };
            return OperationResultGeneric<ProductDto>.Ok(productdto,$"Product '{dto.ProductName}' created successfully.");
        }

        #endregion

        #region Update Method

        public async Task<OperationResultGeneric<ProductDto>> UpdateAsync(UpdateProductDto dto)
        {
            // Check if product exists
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                return OperationResultGeneric<ProductDto>.Fail($"Product with ID {dto.ProductId} not found.");

            // Check if category exists
            var categoryExists = await _categoryRepository.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                return OperationResultGeneric<ProductDto>.Fail($"Category with ID {dto.CategoryId} not found.");

            // Manual Update
            product.Name = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
            
            var productdto = new ProductDto()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Not Exist Category",
            };
            return OperationResultGeneric<ProductDto>.Ok(productdto,$"Product '{dto.ProductName}' updated successfully.");
        }

        #endregion

        #region Delete Method

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return OperationResult.Fail($"Product with ID {id} not found.");

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();

            var productdto =  new ProductDto()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name??"Not Exist Category",
            };
            return OperationResult.Ok($"Product '{product.Name}' deleted successfully.");
        }

        #endregion
    }
}