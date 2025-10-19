public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly EcommerceDbContext _context;
    private bool _disposed = false;

    // Repositories
    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }
    public ICartRepository Carts { get; }
    public ICartItemRepository CartItems { get; }
    public ICouponRepository Coupons { get; }

    public UnitOfWork(
        EcommerceDbContext context,
        IProductRepository products,
        ICategoryRepository categories,
        ICartRepository carts,
        ICartItemRepository cartItems,
        ICouponRepository coupons)
    {
        _context = context;
        Products = products;
        Categories = categories;
        Carts = carts;
        CartItems = cartItems;
        Coupons = coupons;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // ✅ Dispose Implementation
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
