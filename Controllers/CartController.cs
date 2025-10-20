using E_Commerce.Common;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.Models;
using E_Commerce.Service.CartServ;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        #region GET Endpoints

        [HttpGet("GetAllCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            var carts = await _cartService.GetAllCartsAsync();

            if (carts == null)
                return NotFound("No carts found");

            return Ok(carts);
        }

        [HttpGet("GetCartById/{cartId}")]
        public async Task<ActionResult<Cart>> GetCartById(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);

            if (cart == null)
                return NotFound($"Cart with ID {cartId} not found");

            return Ok(cart);
        }

        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<ActionResult<Cart>> GetCartByUserId(string userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
                return NotFound($"Cart for user {userId} not found");

            return Ok(cart);
        }

        [HttpGet("GetCartItemCount/{userId}")]
        public async Task<ActionResult<int>> GetCartItemCount(string userId)
        {
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("GetCartTotal/{userId}")]
        public async Task<ActionResult<CartTotalSaleDto>> GetCartTotal(string userId)
        {
            var cartTotal = await _cartService.GetCartTotalAsync(userId);
            return Ok(cartTotal);
        }

        [HttpPost("CheckProductExists")]
        public async Task<ActionResult<CartDto>> CheckProductExists([FromBody] ProductExistsInCartDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("UserId is required");

            var cartDto = await _cartService.CheckIfProductExistsInCartAsync(dto);

            if (cartDto == null)
                return NotFound("Product not found in cart");

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

            var result = await _cartService.ApplyCouponAsync(dto);

            if (result == null)
                return NotFound("Cart is empty or coupon is invalid");

            return Ok(result);
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required");

            var result = await _cartService.ClearCartAsync(userId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(new { Message = result.Message });
        }

        [HttpPost("ValidateCart")]
        public async Task<ActionResult> ValidateCart([FromBody] CheckoutCartDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("UserId is required");

            var result = await _cartService.ValidateCartBeforeCheckoutAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { Message = result.Message });
        }

        #endregion
    }
}