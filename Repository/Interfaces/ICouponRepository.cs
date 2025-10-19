namespace E_Commerce.Repository.Interfaces
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon?> GetActiveCouponByCodeAsync(string code);

    }
}
