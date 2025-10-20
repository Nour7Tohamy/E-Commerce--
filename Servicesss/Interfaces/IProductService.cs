namespace E_Commerce.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId); 
        Task<IEnumerable<ProductDto>> SearchAsync(string name);
        Task<OperationResultGeneric<ProductDto>> AddAsync(CreateProductDto dto);
        Task<OperationResultGeneric<ProductDto>> UpdateAsync(UpdateProductDto dto);
        Task<OperationResult> DeleteAsync(int id);
    }
}