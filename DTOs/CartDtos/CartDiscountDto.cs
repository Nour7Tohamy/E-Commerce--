namespace E_Commerce.DTOs.CartItemDtos
{
    public class CartDiscountDto
    {
        public int cartId { get; set; }
        public string userId { get; set; }
        public string couponCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAfterDiscount { get; set; }
    }
}
