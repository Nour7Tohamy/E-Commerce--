namespace E_Commerce.DTOs.OrderDtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 
        public string PaymentMethod { get; set; }
        public string OrderType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Total { get; set; }
        public string? CustomerName { get; set; }
        public string? DeliveryAddress { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
