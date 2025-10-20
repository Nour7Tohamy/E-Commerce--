namespace E_Commerce.DTOs.OrderDtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        // اسم المنتج
        public string ProductName { get; set; }

        // عدد القطع المطلوبة
        public int Quantity { get; set; }

        // السعر الفردي للمنتج
        public decimal Price { get; set; }

        // إجمالي السعر (Quantity × Price)
        public decimal Total => Quantity * Price;
    }
}
