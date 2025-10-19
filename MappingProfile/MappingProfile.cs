using AutoMapper;
using E_Commerce.Models;
using E_Commerce.DTOs.CartDtos;
using E_Commerce.DTOs.CartItemDtos;
using E_Commerce.DTOs.CategoryDtos;
using E_Commerce.DTOs.ProductDtos;

namespace E_Commerce.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================
            // Product Mappings
            // ============================================
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Not Exist"));

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? "No Description Exist"))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            //// Mapping for products within category DTOs
            //CreateMap<ProductInCategoryDto, Product>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId ?? 0))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
            //    .ForMember(dest => dest.Category, opt => opt.Ignore())
            //    .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            //    .ForMember(dest => dest.CartItems, opt => opt.Ignore())
            //    .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            // ============================================
            // Category Mappings
            // ============================================
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Category, CategoryWithProductsDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Category, CategoryWithCountDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

            CreateMap<UpdateCategoryWithProductsDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<CreateCategoryWithProductsDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            // ============================================
            // Cart Mappings
            // ============================================
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductDescription, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

            CreateMap<Cart, CartTotalSaleDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TotalSales, opt => opt.MapFrom(src => 
                    src.CartItems.Sum(ci => ci.Quantity * ci.Product.Price)));

            CreateMap<Cart, CartDiscountDto>()
                .ForMember(dest => dest.cartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.couponCode, opt => opt.MapFrom(src => src.Coupon != null ? src.Coupon.Code : null))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Coupon != null ? src.Coupon.Percentage : 0))
                .ForMember(dest => dest.TotalAfterDiscount, opt => opt.Ignore());

            // ============================================
            // CartItem Mappings
            // ============================================
            CreateMap<CartItem, CartItemDto>()
                .ReverseMap();

            // ============================================
            // Coupon Mappings
            // ============================================
            CreateMap<Coupon, ApplyCouponRequestDto>()
                .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // ============================================
            // Order Mappings (if needed)
            // ============================================
            //CreateMap<Order, OrderDto>().ReverseMap();
            //CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            //// ============================================
            //// User Mappings (if needed)
            //// ============================================
            //CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}