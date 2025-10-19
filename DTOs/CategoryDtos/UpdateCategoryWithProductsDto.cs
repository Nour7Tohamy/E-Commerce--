namespace E_Commerce.DTOs.CategoryDtos
{
    public class UpdateCategoryWithProductsDto
    {
        public string CategoryName { get; set; }

        public List<UpdateProductDto> Products { get; set; } = new();
    }
}

