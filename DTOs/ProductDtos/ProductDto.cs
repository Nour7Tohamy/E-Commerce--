namespace E_Commerce.DTOs.ProductDtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string? Description { get; set; }
      
        public string Status { get; set; }

        public decimal Price { get; set; }

        public int ?CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
