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

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _cartRepository.GetAllCartsAsync();
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _cartRepository.GetCartByIdAsync(cartId);
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _cartRepository.GetCartItemCountAsync(userId);
        }

        public async Task<CartTotalSaleDto> GetCartTotalAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new CartTotalSaleDto
                {
                    UserId = userId,
                    TotalSales = 0
                };
            }

            var total = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Manual Mapping
            return new CartTotalSaleDto
            {
                CartId = cart.Id,
                UserId = userId,
                TotalSales = total
            };
        }

        public async Task<CartDto?> CheckIfProductExistsInCartAsync(ProductExistsInCartDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return null;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (cartItem == null)
                return null;

            // Manual Mapping
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

        #endregion

        #region Business Logic Methods

        public async Task<CartDiscountDto?> ApplyCouponAsync(ApplyCouponRequestDto dto)
        {
            // Get cart
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return null;

            // Get coupon
            var coupon = await _couponRepository.GetValidCouponByCodeAsync(dto.CouponCode);

            if (coupon == null)
                return null;

            // Apply coupon to cart
            cart.CouponId = coupon.Id;
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            // Calculate totals
            var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            var discountedTotal = total * (1 - coupon.Percentage / 100m);

            // Manual Mapping
            return new CartDiscountDto
            {
                cartId = cart.Id,
                userId = cart.UserId,
                couponCode = coupon.Code,
                DiscountPercentage = coupon.Percentage,
                TotalAfterDiscount = discountedTotal
            };
        }

        public async Task<OperationResult> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                return OperationResult.Fail("Cart not found");

            // Delete cart items first
            await _cartRepository.DeleteCartItemsAsync(cart.Id);

            // Delete cart
            _cartRepository.Delete(cart);
            await _cartRepository.SaveChangesAsync();

            return OperationResult.Ok("Cart cleared successfully");
        }

        public async Task<OperationResult> ValidateCartBeforeCheckoutAsync(CheckoutCartDto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            // Validate cart exists and has items
            if (cart == null || !cart.CartItems.Any())
                return OperationResult.Fail("Cart or CartItems is empty");

            // Validate stock availability
            if (cart.CartItems.Any(ci => ci.Quantity > ci.Product.Stock))
                return OperationResult.Fail("Some products are out of stock");

            // Validate coupon if provided
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

            // Validate expected total if provided
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