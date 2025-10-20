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

        [Range(0, double.MaxValue)]
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }
    }
}
