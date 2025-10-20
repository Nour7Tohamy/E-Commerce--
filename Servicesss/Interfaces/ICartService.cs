using E_Commerce.Common;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.Models;

namespace E_Commerce.Service.CartServ
{
    public interface ICartService
    {
        // Get Methods
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<CartTotalSaleDto> GetCartTotalAsync(string userId);
        Task<CartDto?> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto);

        // Business Logic Methods
        Task<CartDiscountDto?> ApplyCouponAsync(ApplyCouponRequestDto dto);
        Task<OperationResult> ClearCartAsync(string userId);
        Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto);
    }
}