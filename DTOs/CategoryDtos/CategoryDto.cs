namespace E_Commerce.DTOs.CategoryDtos
{
    public class CategoryDto
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; }

        [StringLength(200, ErrorMessage = "Description can't exceed 200 characters.")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string? ImageUrl { get; set; }
    }

}
