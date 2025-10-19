
namespace E_Commerce.Repository.Classes
{
    public class CartItemRepository : GenericRepository<CartItem> , ICartItemRepository
    {
        public CartItemRepository(EcommerceDbContext context) : base(context)
        {
        }

    }

}
