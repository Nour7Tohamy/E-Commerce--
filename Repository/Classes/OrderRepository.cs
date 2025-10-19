using E_Commerce.Repository.Interfaces;

namespace E_Commerce.Repository.Classes
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(EcommerceDbContext context) : base(context)
        {
        }
    }
}
