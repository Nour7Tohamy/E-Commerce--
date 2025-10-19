using AutoMapper;
using E_Commerce.Common;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.Service.CartServ;

namespace E_Commerce.Service.Classes
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartTotalSaleDto> GetCartTotalAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return new CartTotalSaleDto { UserId = userId, TotalSales = 0 };

            var total = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            return new CartTotalSaleDto
            {
                CartId = cart.Id,
                UserId = userId,
                TotalSales = total
            };
        }
        public async Task<CartDiscountDto?> ApplyCouponAsync(ApplyCouponRequestDto dto)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(dto.UserId);
            if (cart == null || !cart.CartItems.Any())
                return null;
            var coupon = await _unitOfWork.Coupons
            //error in
                .FirstOrDefaultAsync(c => c.Code == dto.CouponCode && c.IsActive && c.ExpiryDate > DateTime.UtcNow);

            if (coupon == null) return null;

            if (coupon == null) return null;

            cart.CouponId = coupon.Id;
            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync();

            var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            var discountedTotal = total * (1 - coupon.Percentage / 100m);

            return new CartDiscountDto
            {
                cartId = cart.Id,
                userId = cart.UserId,
                couponCode = coupon.Code,
                DiscountPercentage = coupon.Percentage,
                TotalAfterDiscount = discountedTotal
            };
        }
        public async Task<CartDto?> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(dto.UserId);
            if (cart == null || !cart.CartItems.Any())
                return null;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (cartItem == null) return null;

            return new CartDto
            {
                CartId = cart.Id,
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                ProductDescription = cartItem.Product?.Description ?? "No description available",
                Quantity = cartItem.Quantity,
                TotalPrice = (cartItem.Product?.Price ?? 0m) * cartItem.Quantity
            };
        }
        public async Task<OperationResult> ClearCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
                return OperationResult.Fail("Cart not found");

            _unitOfWork.CartItems.DeleteRange(cart.CartItems); //error in
            _unitOfWork.Carts.Delete(cart);

            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok("Cart cleared successfully");
        }
        public async Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(dto.UserId);
            if (cart == null || !cart.CartItems.Any())
                return OperationResult.Fail("Cart or CartItems is empty");

            if (cart.CartItems.Any(ci => ci.Quantity > ci.Product.Stock))
                return OperationResult.Fail("Some products are out of stock");

            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                if (cart.Coupon == null || cart.Coupon.Code != dto.CouponCode ||
                    !cart.Coupon.IsActive || cart.Coupon.ExpiryDate <= DateTime.UtcNow)
                {
                    return OperationResult.Fail("Coupon is invalid or expired");
                }
            }

            if (dto.ExpectedTotal.HasValue)
            {
                var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
                if (total != dto.ExpectedTotal.Value)
                    return OperationResult.Fail("Cart total has changed");
            }

            return OperationResult.Ok("Your cart is valid and ready for checkout");
        }
    }
}
