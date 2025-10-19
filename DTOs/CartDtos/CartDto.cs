using System;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTOs.CartDtos
{
    public class CartDto
    {
        [Required]
        public int CartId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string ProductDescription { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Url(ErrorMessage = "Invalid image URL format.")]
        public string? ProductImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
