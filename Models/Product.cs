using E_Commerce.Enums;

namespace E_Commerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int? count { get; set; }
        public int Stock {  get; set; }

        [Required]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Available;


        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}