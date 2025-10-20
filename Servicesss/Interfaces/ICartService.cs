namespace E_Commerce.Service.CartServ
{
    public interface ICartService
    {
        // Get Methods
        Task<OperationResultGeneric<IEnumerable<Cart>>> GetAllCartsAsync();
        Task<OperationResultGeneric<Cart>> GetCartByIdAsync(int cartId);
        Task<OperationResultGeneric<Cart>> GetCartByUserIdAsync(string userId);
        Task<OperationResultGeneric<int>> GetCartItemCountAsync(string userId);
        Task<OperationResultGeneric<CartTotalSaleDto>> GetCartTotalAsync(string userId);
        Task<OperationResultGeneric<CartDto>> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto);

        // Business Logic Methods
        Task<OperationResultGeneric<CartDiscountDto>> ApplyCouponAsync(ApplyCouponRequestDto dto);
        Task<OperationResult> ClearCartAsync(string userId);
        Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto);
    }
}