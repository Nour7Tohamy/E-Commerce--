using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository._Generics;
using E_Commerce.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository.Classes
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly EcommerceDbContext _context;

        public CartRepository(EcommerceDbContext context) : base(context)
        {
            this._context = context;
        }

        public virtual async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public virtual async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Include(c => c.Coupon)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public virtual async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.Cart.UserId == userId)
                .CountAsync();
        }
      

        public async Task DeleteCartItemsAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}