using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Common.Exceptions;
using ShopApi.DTOs.Cart;
using ShopApi.Models;
using System.Security.Claims;

namespace ShopApi.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public CartService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        // Lay userId hien tai tu claims trong access token.
        private int GetCurrentUserId()
        {
            var userIdClaim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            return int.Parse(userIdClaim.Value);
        }

        // Lay gio hang cua user; neu chua co thi tao gio hang moi.
        private async Task<Cart> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // Them san pham vao gio hang va cap nhat so luong/ don gia.
        public async Task AddToCart(AddToCartDto dto)
        {
            if (dto.Quantity <= 0)
                throw new AppBadRequestException("So luong phai lon hon 0");

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == dto.ProductId && x.IsActive);
            if (product == null)
                throw new AppNotFoundException("Khong tim thay san pham");

            var userId = GetCurrentUserId();
            var cart = await GetCart(userId);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (item != null)
            {
                var newQuantity = item.Quantity + dto.Quantity;
                if (newQuantity > product.StockQuantity)
                    throw new AppBadRequestException("Khong du ton kho");

                item.Quantity += dto.Quantity;
                item.UnitPrice = product.Price;
            }
            else
            {
                if (dto.Quantity > product.StockQuantity)
                    throw new AppBadRequestException("Khong du ton kho");

                cart.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price
                });
            }

            await _context.SaveChangesAsync();
        }

        // Cap nhat so luong cua mot item trong gio hang.
        public async Task UpdateItem(int itemId, int quantity)
        {
            if (quantity <= 0)
                throw new AppBadRequestException("So luong phai lon hon 0");

            var userId = GetCurrentUserId();

            var item = await _context.CartItems
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Cart.UserId == userId);

            if (item == null) throw new AppNotFoundException("Khong tim thay san pham trong gio");
            if (item.Product == null) throw new AppNotFoundException("Khong tim thay san pham");
            if (quantity > item.Product.StockQuantity)
                throw new AppBadRequestException("Khong du ton kho");

            item.Quantity = quantity;
            item.UnitPrice = item.Product.Price;
            await _context.SaveChangesAsync();
        }

        // Xoa item khoi gio hang.
        public async Task RemoveItem(int itemId)
        {
            var userId = GetCurrentUserId();

            var item = await _context.CartItems
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Cart.UserId == userId);

            if (item == null) throw new AppNotFoundException("Khong tim thay san pham trong gio");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        // Lay chi tiet gio hang de tra ve cho API.
        public async Task<CartResponseDto> GetCartDetail()
        {
            var userId = GetCurrentUserId();
            var cart = await GetCart(userId);

            var items = cart.Items.Select(x => new
            {
                x.Id,
                x.ProductId,
                x.Product.Name,
                UnitPrice = x.UnitPrice,
                x.Quantity,
                Total = x.UnitPrice * x.Quantity
            }).ToList();

            var total = items.Sum(x => x.Total);

            return new CartResponseDto
            {
                Items = items.Cast<object>().ToList(),
                TotalPrice = total
            };
        }
    }
}



