using E_Commerce.DTOs.ProductDtos;
using E_Commerce.Common;

namespace E_Commerce.Service.CartServerices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> SearchAsync(string name);
        Task<OperationResult> AddAsync(CreateProductDto dto);
        Task<OperationResult> UpdateAsync(UpdateProductDto dto);
        Task<OperationResult> DeleteAsync(int id);
    }
}
