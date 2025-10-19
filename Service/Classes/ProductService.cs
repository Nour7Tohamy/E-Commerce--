using AutoMapper;
using E_Commerce.Common;
using E_Commerce.DTOs.ProductDtos;

using E_Commerce.Service.Interfaces;

namespace E_Commerce.Service.Classes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get Methods

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdWithCategoryAsync(id);
            return _mapper.Map<ProductDto?>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId)
        {
            var exists = await _unitOfWork.Categories.ExistsAsync(categoryId);
            if (!exists)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");

            var products = await _unitOfWork.Products.GetAllProductsByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string name)
        {
            var products = await _unitOfWork.Products.SearchProductsAsync(name);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        #endregion

        #region Create Method

        public async Task<OperationResult> AddAsync(CreateProductDto dto)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                return OperationResult.Fail($"Category with ID {dto.CategoryId} not found.");

            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok($"Product '{dto.ProductName}' created successfully.");
        }

        #endregion

        #region Update Method

        public async Task<OperationResult> UpdateAsync(UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                return OperationResult.Fail($"Product with ID {dto.ProductId} not found.");

            var categoryExists = await _unitOfWork.Categories.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                return OperationResult.Fail($"Category with ID {dto.CategoryId} not found.");

            _mapper.Map(dto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok($"Product '{dto.ProductName}' updated successfully.");
        }

        #endregion

        #region Delete Method

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return OperationResult.Fail($"Product with ID {id} not found.");

             _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok($"Product '{product.Name}' deleted successfully.");
        }

        #endregion
    }
}
