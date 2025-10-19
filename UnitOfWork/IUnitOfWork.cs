public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    ICartRepository Carts { get; }
    ICartItemRepository CartItems { get; }
    ICouponRepository Coupons { get; }

    Task<int> SaveChangesAsync();
}
