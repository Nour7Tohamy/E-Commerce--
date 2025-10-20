using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository._Generics;
using E_Commerce.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository.Classes
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        private readonly EcommerceDbContext _context;

        public CouponRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Coupon?> GetValidCouponByCodeAsync(string code)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c =>
                    c.Code == code &&
                    c.IsActive &&
                    c.ExpiryDate > DateTime.UtcNow);
        }
    }
}