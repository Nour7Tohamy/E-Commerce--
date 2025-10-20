namespace E_Commerce.DTOs.OrderDtos
{
    //user
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderType { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? CustomerName { get; set; }
        public List<UpdateOrderItemDto>? Items { get; set; } = new();
    }


}
