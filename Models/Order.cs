using E_Commerce.Models.Enums;

namespace E_Commerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending; 

        [Required]
        public string PaymentMethod { get; set; } // طريقه الدفع

        [Required]
        public string OrderType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; } // ضريبه

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; } // رسوم التوصيل

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string? DeliveryAddress { get; set; } // مكان الاستلام

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
