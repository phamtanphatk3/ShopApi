using Microsoft.EntityFrameworkCore;
using ShopApi.Common;
using ShopApi.Models;

namespace ShopApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Seed only when DB is empty enough; keeps user data safe.
            // Chi can co du lieu san pham thi xem nhu DB da duoc khoi tao.
            // Truong hop DB co Users (tu test/login) nhung Products trong -> van can seed.
            if (await context.Products.AnyAsync())
                return;

            var now = DateTime.UtcNow;

            // Upsert users by Username to avoid duplicates when DB already has users.
            async Task<User> EnsureUserAsync(string username, string role)
            {
                var existed = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (existed != null)
                    return existed;

                var user = new User
                {
                    Username = username,
                    Password = PasswordHelper.HashPassword("123"),
                    Role = role
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user;
            }

            // Upsert categories by Slug because Category seed also exists in OnModelCreating.
            async Task<Category> EnsureCategoryAsync(string name, string slug)
            {
                var existed = await context.Categories.FirstOrDefaultAsync(x => x.Slug == slug);
                if (existed != null)
                    return existed;

                var cat = new Category { Name = name, Slug = slug, IsActive = true };
                context.Categories.Add(cat);
                await context.SaveChangesAsync();
                return cat;
            }

            var admin = await EnsureUserAsync("admin", "Admin");
            var staff = await EnsureUserAsync("staff", "Staff");
            var customer = await EnsureUserAsync("customer", "Customer");

            var catPhone = await EnsureCategoryAsync("Điện thoại", "dien-thoai");
            var catLaptop = await EnsureCategoryAsync("Laptop", "laptop");
            var catAccessory = await EnsureCategoryAsync("Phụ kiện", "phu-kien");

            var p1 = new Product
            {
                Name = "iPhone 15 128GB",
                SKU = "IP15-128",
                Brand = "Apple",
                Price = 19990000m,
                StockQuantity = 50,
                CategoryId = catPhone.Id,
                Description = "Điện thoại Apple chính hãng.",
                CreatedAt = now
            };
            var p2 = new Product
            {
                Name = "Samsung Galaxy S24 256GB",
                SKU = "SS-S24-256",
                Brand = "Samsung",
                Price = 21990000m,
                StockQuantity = 40,
                CategoryId = catPhone.Id,
                Description = "Điện thoại Samsung chính hãng.",
                CreatedAt = now
            };
            var p3 = new Product
            {
                Name = "Laptop Dell Inspiron 14",
                SKU = "DELL-IN14",
                Brand = "Dell",
                Price = 16990000m,
                StockQuantity = 20,
                CategoryId = catLaptop.Id,
                Description = "Laptop học tập và văn phòng.",
                CreatedAt = now
            };
            var p4 = new Product
            {
                Name = "Sạc nhanh 65W Type-C",
                SKU = "CHG-65W-USBC",
                Brand = "Anker",
                Price = 590000m,
                StockQuantity = 100,
                CategoryId = catAccessory.Id,
                Description = "Sạc nhanh PD 65W.",
                CreatedAt = now
            };

            context.Products.AddRange(p1, p2, p3, p4);
            await context.SaveChangesAsync();

            context.ProductSpecifications.AddRange(
                new ProductSpecification { ProductId = p1.Id, Key = "ROM", Value = "128GB" },
                new ProductSpecification { ProductId = p1.Id, Key = "Màn hình", Value = "6.1 inch" },
                new ProductSpecification { ProductId = p2.Id, Key = "ROM", Value = "256GB" },
                new ProductSpecification { ProductId = p2.Id, Key = "Màn hình", Value = "6.2 inch" },
                new ProductSpecification { ProductId = p3.Id, Key = "RAM", Value = "16GB" },
                new ProductSpecification { ProductId = p3.Id, Key = "SSD", Value = "512GB" }
            );

            context.ProductImages.AddRange(
                new ProductImage { ProductId = p1.Id, ImageUrl = "/images/brands/apple-main.jpg", IsMain = true, SortOrder = 0 },
                new ProductImage { ProductId = p2.Id, ImageUrl = "/images/brands/samsung-main.jpg", IsMain = true, SortOrder = 0 },
                new ProductImage { ProductId = p4.Id, ImageUrl = "/images/0be542bf-2926-43fe-8739-886d4d6f3dcb.jfif", IsMain = true, SortOrder = 0 }
            );

            context.ProductRegionPrices.AddRange(
                new ProductRegionPrice { ProductId = p1.Id, Region = "HCM", Price = 19890000m },
                new ProductRegionPrice { ProductId = p1.Id, Region = "HN", Price = 19950000m },
                new ProductRegionPrice { ProductId = p2.Id, Region = "HCM", Price = 21890000m },
                new ProductRegionPrice { ProductId = p3.Id, Region = "HCM", Price = 16890000m }
            );

            context.InventoryTransactions.AddRange(
                new InventoryTransaction { ProductId = p1.Id, Quantity = 50, Type = "IMPORT", CreatedAt = DateTime.Now },
                new InventoryTransaction { ProductId = p2.Id, Quantity = 40, Type = "IMPORT", CreatedAt = DateTime.Now },
                new InventoryTransaction { ProductId = p3.Id, Quantity = 20, Type = "IMPORT", CreatedAt = DateTime.Now },
                new InventoryTransaction { ProductId = p4.Id, Quantity = 100, Type = "IMPORT", CreatedAt = DateTime.Now }
            );

            var store1 = new Store
            {
                Name = "ShopApi HCM - Q.1",
                Province = "Hồ Chí Minh",
                District = "Quận 1",
                Address = "1 Nguyễn Huệ",
                Latitude = 10.7758,
                Longitude = 106.7033
            };
            var store2 = new Store
            {
                Name = "ShopApi HN - Hoàn Kiếm",
                Province = "Hà Nội",
                District = "Hoàn Kiếm",
                Address = "10 Tràng Tiền",
                Latitude = 21.0285,
                Longitude = 105.8542
            };
            context.Stores.AddRange(store1, store2);
            await context.SaveChangesAsync();

            context.StoreInventories.AddRange(
                new StoreInventory { StoreId = store1.Id, ProductId = p1.Id, Quantity = 20 },
                new StoreInventory { StoreId = store1.Id, ProductId = p2.Id, Quantity = 15 },
                new StoreInventory { StoreId = store1.Id, ProductId = p3.Id, Quantity = 8 },
                new StoreInventory { StoreId = store2.Id, ProductId = p1.Id, Quantity = 10 },
                new StoreInventory { StoreId = store2.Id, ProductId = p2.Id, Quantity = 10 },
                new StoreInventory { StoreId = store2.Id, ProductId = p4.Id, Quantity = 40 }
            );

            var promo = new Promotion
            {
                Name = "Khuyến mãi khai trương",
                DiscountType = "Percent",
                DiscountValue = 10m,
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today.AddDays(30),
                IsActive = true
            };
            context.Promotions.Add(promo);
            await context.SaveChangesAsync();

            context.ProductPromotions.AddRange(
                new ProductPromotion { ProductId = p1.Id, PromotionId = promo.Id },
                new ProductPromotion { ProductId = p2.Id, PromotionId = promo.Id }
            );

            context.Coupons.AddRange(
                new Coupon
                {
                    Code = "WELCOME10",
                    DiscountType = "Percent",
                    DiscountValue = 10m,
                    MinOrderValue = 1000000m,
                    UsageLimit = 100,
                    UsedCount = 0,
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today.AddDays(60)
                },
                new Coupon
                {
                    Code = "SHIPFREE",
                    DiscountType = "Fixed",
                    DiscountValue = 30000m,
                    MinOrderValue = 300000m,
                    UsageLimit = 200,
                    UsedCount = 0,
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today.AddDays(60)
                }
            );

            // Sample cart + order + warranty + installment
            var cart = new Cart { UserId = customer.Id, CreatedAt = now };
            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            context.CartItems.AddRange(
                new CartItem { CartId = cart.Id, ProductId = p4.Id, Quantity = 2, UnitPrice = p4.Price },
                new CartItem { CartId = cart.Id, ProductId = p1.Id, Quantity = 1, UnitPrice = p1.Price }
            );

            var order = new Order
            {
                UserId = customer.Id,
                OrderCode = $"OD{DateTime.Now:yyyyMMddHHmmss}",
                CustomerName = "Nguyễn Văn A",
                CreatedAt = DateTime.Now,
                Status = "Paid"
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var oi1 = new OrderItem { OrderId = order.Id, ProductId = p1.Id, Quantity = 1, UnitPrice = p1.Price, LineTotal = p1.Price };
            var oi2 = new OrderItem { OrderId = order.Id, ProductId = p4.Id, Quantity = 1, UnitPrice = p4.Price, LineTotal = p4.Price };
            context.OrderItems.AddRange(oi1, oi2);
            order.FinalAmount = oi1.LineTotal + oi2.LineTotal;

            context.WarrantyRecords.Add(
                new WarrantyRecord
                {
                    SerialNumber = "SN-SEED-000001",
                    CustomerPhone = "0900000000",
                    OrderId = order.Id,
                    ProductId = p1.Id,
                    WarrantyStartDate = DateTime.Today,
                    WarrantyEndDate = DateTime.Today.AddYears(1),
                    Status = "InWarranty",
                    CreatedAt = now
                }
            );

            context.InstallmentRequests.Add(
                new InstallmentRequest
                {
                    ProductId = p2.Id,
                    ProductPrice = p2.Price,
                    Months = 12,
                    DownPayment = 5000000m,
                    MonthlyPayment = Math.Round((p2.Price - 5000000m) / 12m, 0),
                    CustomerName = "Trần Thị B",
                    Phone = "0911111111",
                    CreatedAt = DateTime.Now
                }
            );

            await context.SaveChangesAsync();
        }
    }
}
