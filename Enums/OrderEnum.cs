namespace E_Commerce.Enums
{
    public enum OrderStatus
    {
        Pending = 0,       // تم إنشاء الطلب ولم يُراجع بعد
        Processing = 1,    // الطلب جاري تجهيزه
        Shipped = 2,       // الطلب خرج للتوصيل
        Delivered = 3,     // تم التسليم
        Cancelled = 4      // تم الإلغاء
    }
}
