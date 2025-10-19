namespace E_Commerce.DTOs.CategoryDtos
{
    public class CategoryWithProductsDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
