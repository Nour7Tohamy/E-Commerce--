namespace E_Commerce.DTOs.OrderDtos
{
    public class CreateOrderDto
    {

        public string UserId { get; set; }
        public string PaymentMethod { get; set; } // طريقه الدفع
        public string OrderType { get; set; } // مثلاً Delivery أو Pickup
        public string DeliveryAddress { get; set; } // مكان الاستلام
        public string? CustomerName { get; set; }

        // المستخدم بيبعت العناصر اللي طلبها
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

}
