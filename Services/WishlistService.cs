using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Wishlist;
using ShopApi.Models;
using System.Security.Claims;

namespace ShopApi.Services
{
    public class WishlistService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public WishlistService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        private int GetCurrentUserId()
        {
            var claim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            return int.Parse(claim.Value);
        }

        public async Task<List<WishlistItemDto>> GetMyWishlistAsync()
        {
            var userId = GetCurrentUserId();

            var items = await _context.WishlistItems
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .Include(x => x.Product.Images)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return items
                .Select(x => new WishlistItemDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    SKU = x.Product.SKU,
                    Brand = x.Product.Brand,
                    Price = x.Product.Price,
                    ImageUrl = x.Product.Images
                        .Where(i => i.IsMain)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();
        }

        public async Task<object> AddAsync(int productId)
        {
            var userId = GetCurrentUserId();

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new AppNotFoundException("Khong tim thay san pham");

            var exists = await _context.WishlistItems.AnyAsync(x => x.UserId == userId && x.ProductId == productId);
            if (exists)
                throw new AppConflictException("San pham da co trong danh sach yeu thich");

            _context.WishlistItems.Add(new WishlistItem
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return new { productId };
        }

        public async Task<object> RemoveAsync(int productId)
        {
            var userId = GetCurrentUserId();

            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            if (item == null)
                throw new AppNotFoundException("Khong tim thay san pham trong danh sach yeu thich");

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return new { productId };
        }
    }
}
