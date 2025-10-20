using E_Commerce.Repository._Generics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Commerce.Service.Classes
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        private readonly IGenericRepository<Order> _repo;
        private readonly IOrderRepository _orderrepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IGenericRepository<Order> repository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork) // تمرير repo + unitOfWork للـ GenericService
        {
            this._repo = repository;
            this._orderrepo = orderRepository;
            this._unitOfWork = unitOfWork;
        }

        // ========== Admin ==========
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = await _orderrepo.GetAllAsync();
            orders.OrderByDescending(o => o.OrderDate);
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderrepo.GetByIdAsync(orderId);
            return order;
        }

        public async Task UpdateOrderStatusAsync(UpdateOrderDto dto)
        {    
        }

        // ========== User ==========
        public Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetUserOrderByIdAsync(string userId, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrderAsync(CreateOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(UpdateOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrderAsync(string userId, int orderId)
        {
            throw new NotImplementedException();
        }
    }
}