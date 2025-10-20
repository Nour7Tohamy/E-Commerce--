namespace E_Commerce.DTOs.OrderDtos
{
    //Admin
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; } 
    }
}
