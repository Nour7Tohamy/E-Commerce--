using E_Commerce.Models;

namespace E_Commerce.Repository.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<int> GetCartItemCountAsync(string userId);
        Task<Cart> GetCartByIdAsync(int cartId);
    }
}