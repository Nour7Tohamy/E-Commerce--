namespace E_Commerce.DTOs.CategoryDtos
{
    public class CategoryWithCountDto
    {
        [Required]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(50, ErrorMessage = "Category name must be less than 50 characters.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Total Product Count")]
        [Range(0, int.MaxValue, ErrorMessage = "Product count cannot be negative.")]
        public int ProductCount { get; set; }
    }
}
