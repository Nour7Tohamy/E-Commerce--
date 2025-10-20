using E_Commerce.Models;
using E_Commerce.Repository._Generics;

namespace E_Commerce.Repository.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderForUserByIdAsync(string userId , int orderId);
        Task SaveChangesAsync();
    }
}