using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(EcommerceDbContext context, UserManager<ApplicationUser> userManager)
        {
            // 🔹 تأكد من وجود قاعدة البيانات وتطبيق الـ Migrations
            await context.Database.MigrateAsync();

            // 🔹 لو فيه بيانات بالفعل .. اخرج
            //if (context.Categories.Any() && context.Products.Any())
            //    return;

            // ===========================================================
            // 🟩 1️⃣ Seed Categories
            // ===========================================================
            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing" },
                new Category { Name = "Books" },
                new Category { Name = "Home & Garden" },
                new Category { Name = "Sports" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // ===========================================================
            // 🟦 2️⃣ Re-fetch Categories to get their actual IDs
            // ===========================================================
            var savedCategories = await context.Categories.ToListAsync();

            // ===========================================================
            // 🟨 3️⃣ Seed Products
            // ===========================================================
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop",
                    Price = 15000m,
                    CategoryId = savedCategories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "Smartphone",
                    Price = 8000m,
                    CategoryId = savedCategories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "Headphones",
                    Price = 500m,
                    CategoryId = savedCategories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "T-Shirt",
                    Price = 200m,
                    CategoryId = savedCategories.First(c => c.Name == "Clothing").Id
                },
                new Product
                {
                    Name = "Jeans",
                    Price = 600m,
                    CategoryId = savedCategories.First(c => c.Name == "Clothing").Id
                },
                new Product
                {
                    Name = "C# Programming Book",
                    Price = 350m,
                    CategoryId = savedCategories.First(c => c.Name == "Books").Id
                },
                new Product
                {
                    Name = "Chair",
                    Price = 1200m,
                    CategoryId = savedCategories.First(c => c.Name == "Home & Garden").Id
                },
                new Product
                {
                    Name = "Football",
                    Price = 250m,
                    CategoryId = savedCategories.First(c => c.Name == "Sports").Id
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // ===========================================================
            // 🟧 4️⃣ Seed Test User + Cart
            // ===========================================================
            var testUser = await userManager.FindByEmailAsync("testuser@example.com");
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = "testuser@example.com",
                    Email = "testuser@example.com",
                    EmailConfirmed = true,
                    FullName = "Test User"
                };

                var result = await userManager.CreateAsync(testUser, "Test@123");

                if (result.Succeeded)
                {
                    var cart = new Cart { UserId = testUser.Id };
                    await context.Carts.AddAsync(cart);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
