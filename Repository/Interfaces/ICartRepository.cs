using E_Commerce.Models;
using E_Commerce.Repository._Generics;

namespace E_Commerce.Repository.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task DeleteCartItemsAsync(int cartId); 
        Task SaveChangesAsync(); 
    }
}