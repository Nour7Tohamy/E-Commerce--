using E_Commerce.DTOs.CartItemDtos;
namespace E_Commerce.DTOs.CartDtos
{
    public class CheckoutCartDto
    {
        public string UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public string? CouponCode { get; set; }
        public decimal? ExpectedTotal { get; set; }
    }
}