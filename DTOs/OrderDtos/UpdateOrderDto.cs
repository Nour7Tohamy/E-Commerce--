namespace E_Commerce.DTOs.OrderDtos
{
    //user
    public class UpdateOrderDto
    {
        public string? DeliveryAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderType { get; set; } // مثلاً: Delivery أو Pickup

        // المستخدم ممكن يغيّر أو يمسح Items قبل التأكيد
        public List<UpdateOrderItemDto>? Items { get; set; } = new();
    }

  
}
