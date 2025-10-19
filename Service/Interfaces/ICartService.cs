using E_Commerce.Common;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;

namespace E_Commerce.Service.CartServ
{
    public interface ICartService
    {
        Task<CartTotalSaleDto> GetCartTotalAsync(string userId);
        Task<CartDiscountDto?> ApplyCouponAsync(ApplyCouponRequestDto dto);
        Task<CartDto?> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto);
        Task<OperationResult> ClearCartAsync(string userId);
        Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto);
    }
}
