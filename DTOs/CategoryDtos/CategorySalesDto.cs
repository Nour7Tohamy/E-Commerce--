namespace E_Commerce.DTOs.CategoryDtos
{
    public class CategorySalesDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string CategoryName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TotalSold must be a positive number.")]
        public decimal TotalSales { get; set; }
    }
}
