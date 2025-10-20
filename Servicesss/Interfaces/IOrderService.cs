namespace E_Commerce.Service.Interfaces
{
    public interface IOrderService
    {
        // Admin Operations
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<OperationResultGeneric<OrderDto>> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);

        // User Operations
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetUserOrderByIdAsync(string UserId, int OrderId);
        Task<OperationResultGeneric<OrderDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<OperationResultGeneric<OrderDto>> UpdateOrderAsync(UpdateOrderDto dto);
        Task<OperationResultGeneric<OrderDto>> DeleteOrderAsync(string userId, int orderId);
    }
}