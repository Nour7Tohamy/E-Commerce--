using E_Commerce.Data;
using E_Commerce.Mappings;
using E_Commerce.Models;
using E_Commerce.Repository.Classes;
using E_Commerce.Repository.Interfaces;
using E_Commerce.Service.CartServ;
using E_Commerce.Service.CartServerices;
using E_Commerce.Service.Classes;
using E_Commerce.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================================================================
// 🧩 1️ Configure Database (EF Core)
// ================================================================
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ================================================================
// 🔐 2️ Configure Identity
// ================================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole<string>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<EcommerceDbContext>()
.AddDefaultTokenProviders();

// ================================================================
// 🗺️ 3️ Configure AutoMapper
// ================================================================
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ================================================================
// 🧱 4️ Register Repositories - Services
// ================================================================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Services
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
// ================================================================
// 🔧 5️ Register Services
// ================================================================
builder.Services.AddScoped<ICategoryService, CategoryService>();

// ================================================================
// 🌐 6️ Add Controllers + Swagger
// ================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================================================
// 🚀 Build the App
// ================================================================
var app = builder.Build();

// ================================================================
// 🧩 7️ Initialize Database + Seed Data
// ================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcommerceDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        await DbInitializer.Initialize(context, userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Error occurred while seeding the database.");
    }
}

// ================================================================
// 🧭 8️ Middleware
// ================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ================================================================
// ▶️ 9️ Run
// ================================================================
app.Run();