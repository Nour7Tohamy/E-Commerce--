namespace E_Commerce.DTOs.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string CategoryName { get; set; }

        [StringLength(200, ErrorMessage = "Description can't exceed 200 characters.")]
        public string? Description { get; set; }
       
        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string? ImageUrl { get; set; }

    }
}
