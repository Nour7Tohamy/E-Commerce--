using E_Commerce.Common;
using E_Commerce.DTOs.OrderDtos;
using E_Commerce.Models;

namespace E_Commerce.Service.Interfaces
{
    public interface IOrderService
    {
        // Admin Operations
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<OperationResult> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);

        // User Operations
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetUserOrderByIdAsync(string userId, int orderId);
        Task<OperationResult> CreateOrderAsync(CreateOrderDto dto);
        Task<OperationResult> UpdateOrderAsync(string userId, UpdateOrderDto dto);
        Task<OperationResult> DeleteOrderAsync(string userId, int orderId);
    }
}