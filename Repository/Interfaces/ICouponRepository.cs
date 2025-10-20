using E_Commerce.Models;
using E_Commerce.Repository._Generics;

namespace E_Commerce.Repository.Interfaces
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon?> GetValidCouponByCodeAsync(string code);
    }
}