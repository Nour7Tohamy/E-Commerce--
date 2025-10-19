namespace E_Commerce.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
