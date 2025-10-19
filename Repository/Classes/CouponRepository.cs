

namespace E_Commerce.Repository.Classes
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(EcommerceDbContext context) : base(context)
        {
        }

        public Task<Coupon?> GetActiveCouponByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
