using E_Commerce.Data;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using E_Commerce.Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, EcommerceDbContext context, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _context = context;
            _mapper = mapper;
        }

        #region GET Endpoints

        [HttpGet("GetAllCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            var carts = await _cartRepository.GetAllCartsAsync();

            if (carts == null || !carts.Any())
                return NotFound("No carts found");

            return Ok(carts);
        }

        [HttpGet("GetCartById/{cartId}")]
        public async Task<ActionResult<Cart>> GetCartById(int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);

            if (cart == null)
                return NotFound($"Cart with ID {cartId} not found");

            return Ok(cart);
        }

        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<ActionResult<Cart>> GetCartByUserId(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                return NotFound($"Cart for user {userId} not found");

            return Ok(cart);
        }

        [HttpGet("GetCartItemCount/{userId}")]
        public async Task<ActionResult<int>> GetCartItemCount(string userId)
        {
            var count = await _cartRepository.GetCartItemCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("GetCartTotal/{userId}")]
        public async Task<ActionResult<CartTotalSaleDto>> GetCartTotal(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                return Ok(new CartTotalSaleDto { UserId = userId, TotalSales = 0 });

            var cartTotalDto = _mapper.Map<CartTotalSaleDto>(cart);

            return Ok(cartTotalDto);
        }

        [HttpPost("CheckProductExists")]
        public async Task<ActionResult<CartDto>> CheckProductExists([FromBody] ProductExistsInCartDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("UserId is required");

            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return NotFound("Cart is empty or not found");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (cartItem == null)
                return NotFound("Product not found in cart");

            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.ProductId = dto.ProductId;
            cartDto.ProductDescription = cartItem.Product?.Description ?? "No description";
            cartDto.Quantity = cartItem.Quantity;
            cartDto.TotalPrice = (cartItem.Product?.Price ?? 0m) * cartItem.Quantity;

            return Ok(cartDto);
        }

        #endregion

        #region POST / PUT / DELETE Endpoints

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<CartDiscountDto>> ApplyCoupon([FromBody] ApplyCouponRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("UserId is required");

            if (string.IsNullOrWhiteSpace(dto.CouponCode))
                return BadRequest("CouponCode is required");

            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return NotFound("Cart is empty or not found");

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c =>
                    c.Code == dto.CouponCode &&
                    c.IsActive &&
                    c.ExpiryDate > DateTime.UtcNow);

            if (coupon == null)
                return NotFound("Coupon not valid or expired");

            cart.CouponId = coupon.Id;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            var discountedTotal = total * (1 - coupon.Percentage / 100m);

            var discountDto = _mapper.Map<CartDiscountDto>(cart);
            discountDto.couponCode = coupon.Code;
            discountDto.DiscountPercentage = coupon.Percentage;
            discountDto.TotalAfterDiscount = discountedTotal;

            return Ok(discountDto);
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                return NotFound($"Cart for user {userId} not found");

            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Cart for user {userId} cleared successfully" });
        }

        [HttpPost("ValidateCart")]
        public async Task<ActionResult> ValidateCart([FromBody] CheckoutCartDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("UserId is required");

            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return BadRequest("Cart is empty");

            if (cart.CartItems.Any(ci => ci.Quantity > ci.Product.Stock))
                return BadRequest("Some products are out of stock");

            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                if (cart.Coupon == null ||
                    cart.Coupon.Code != dto.CouponCode ||
                    !cart.Coupon.IsActive ||
                    cart.Coupon.ExpiryDate <= DateTime.UtcNow)
                {
                    return BadRequest("Coupon is invalid or expired");
                }
            }

            if (dto.ExpectedTotal.HasValue)
            {
                var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
                if (total != dto.ExpectedTotal.Value)
                    return BadRequest("Cart total has changed");
            }

            return Ok(new { Message = "Cart is valid and ready for checkout" });
        }

        #endregion
    }
}