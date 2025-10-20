using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTOs.OrderDtos
{
    public class OrderDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Order Type")]
        public string OrderType { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Tax")]
        [DataType(DataType.Currency)]
        public decimal Tax { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Delivery Fee")]
        [DataType(DataType.Currency)]
        public decimal DeliveryFee { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }

        [StringLength(200)]
        [Display(Name = "Delivery Address")]
        public string? DeliveryAddress { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
}
