using E_Commerce.Models.Enums;

namespace E_Commerce.DTOs.OrderDtos
{
    //Admin
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; } // Enum, e.g. Pending, Shipped, Delivered
    }
}
