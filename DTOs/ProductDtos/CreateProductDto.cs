namespace E_Commerce.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category Id is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; }
    }
}
