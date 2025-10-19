namespace E_Commerce.DTOs.CategoryDtos
{
    public class CreateCategoryWithProductsDto
    {
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public List<CreateProductDto> Products { get; set; } = new();
    }
}
