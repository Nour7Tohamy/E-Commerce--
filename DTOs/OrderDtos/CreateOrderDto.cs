namespace E_Commerce.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        public string PaymentMethod { get; set; }
        public string OrderType { get; set; } // مثلاً Delivery أو Pickup
        public string DeliveryAddress { get; set; }

        // المستخدم بيبعت العناصر اللي طلبها
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

}
