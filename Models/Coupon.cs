namespace E_Commerce.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
       
        [Precision(5, 2)]
        public decimal Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }

}
