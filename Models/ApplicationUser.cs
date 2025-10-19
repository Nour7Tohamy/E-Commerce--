using Microsoft.AspNetCore.Identity;
namespace E_Commerce.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        [Required, MaxLength(150)]
        public string FullName { get; set; }

        // One-to-One relationship
        public Cart Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}