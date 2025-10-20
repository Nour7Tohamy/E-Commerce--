using E_Commerce.Common;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.Models;
using E_Commerce.Repository.Interfaces;
using E_Commerce.Service.CartServ;

namespace E_Commerce.Service.Classes
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;

        public CartService(
            ICartRepository cartRepository,
            ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
        }

        #region Get Methods

        public async Task<OperationResultGeneric<IEnumerable<Cart>>> GetAllCartsAsync()
        {
            var carts = await _cartRepository.GetAllCartsAsync();
            return OperationResultGeneric<IEnumerable<Cart>>.Ok(carts, "All carts retrieved successfully");
        }

        public async Task<OperationResultGeneric<Cart>> GetCartByIdAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
                return OperationResultGeneric<Cart>.Fail("Cart not found");

            return OperationResultGeneric<Cart>.Ok(cart);
        }

        public async Task<OperationResultGeneric<Cart>> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return OperationResultGeneric<Cart>.Fail("Cart not found for this user");

            return OperationResultGeneric<Cart>.Ok(cart);
        }

        public async Task<OperationResultGeneric<int>> GetCartItemCountAsync(string userId)
        {
            var count = await _cartRepository.GetCartItemCountAsync(userId);
            return OperationResultGeneric<int>.Ok(count);
        }

        public async Task<OperationResultGeneric<CartTotalSaleDto>> GetCartTotalAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                return OperationResultGeneric<CartTotalSaleDto>.Fail("Cart is empty");

            var total = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            var dto = new CartTotalSaleDto
            {
                CartId = cart.Id,
                UserId = userId,
                TotalSales = total
            };

            return OperationResultGeneric<CartTotalSaleDto>.Ok(dto);
        }

        public async Task<OperationResultGeneric<CartDto>> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return OperationResultGeneric<CartDto>.Fail("Cart not found or empty");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (cartItem == null)
                return OperationResultGeneric<CartDto>.Fail("Product not found in cart");

            var result = new CartDto
            {
                CartId = cart.Id,
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                ProductDescription = cartItem.Product?.Description ?? "No description available",
                Quantity = cartItem.Quantity,
                TotalPrice = (cartItem.Product?.Price ?? 0m) * cartItem.Quantity
            };

            return OperationResultGeneric<CartDto>.Ok(result, "Product exists in cart");
        }

        #endregion

        #region Business Logic Methods

        public async Task<OperationResultGeneric<CartDiscountDto>> ApplyCouponAsync(ApplyCouponRequestDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);
            if (cart == null || !cart.CartItems.Any())
                return OperationResultGeneric<CartDiscountDto>.Fail("Cart not found or empty");

            var coupon = await _couponRepository.GetValidCouponByCodeAsync(dto.CouponCode);
            if (coupon == null)
                return OperationResultGeneric<CartDiscountDto>.Fail("Invalid or expired coupon");

            cart.CouponId = coupon.Id;
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            var discountedTotal = total * (1 - coupon.Percentage / 100m);

            var result = new CartDiscountDto
            {
                cartId = cart.Id,
                userId = cart.UserId,
                couponCode = coupon.Code,
                DiscountPercentage = coupon.Percentage,
                TotalAfterDiscount = discountedTotal
            };

            return OperationResultGeneric<CartDiscountDto>.Ok(result, "Coupon applied successfully");
        }

        public async Task<OperationResult> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return OperationResult.Fail("Cart not found");

            await _cartRepository.DeleteCartItemsAsync(cart.Id);
            _cartRepository.Delete(cart);
            await _cartRepository.SaveChangesAsync();

            return OperationResult.Ok("Cart cleared successfully");
        }

        public async Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);
            if (cart == null || !cart.CartItems.Any())
                return OperationResult.Fail("Cart or CartItems is empty");

            if (cart.CartItems.Any(ci => ci.Quantity > ci.Product.Stock))
                return OperationResult.Fail("Some products are out of stock");

            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                if (cart.Coupon == null ||
                    cart.Coupon.Code != dto.CouponCode ||
                    !cart.Coupon.IsActive ||
                    cart.Coupon.ExpiryDate <= DateTime.UtcNow)
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

        #endregion
    }
}
