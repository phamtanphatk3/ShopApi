using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Repositories
{
    // Repository xu ly truy cap du lieu danh muc.
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lay danh sach danh muc dang hoat dong.
        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories
                .Where(x => x.IsActive)
                .ToListAsync();

        // Lay chi tiet danh muc theo id.
        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        // Kiem tra danh muc co san pham dang hoat dong hay khong.
        public async Task<bool> HasProductsAsync(int categoryId)
            => await _context.Products
                .AnyAsync(x => x.CategoryId == categoryId && x.IsActive);

        // Them moi danh muc.
        public async Task AddAsync(Category category)
            => await _context.Categories.AddAsync(category);

        // Cap nhat danh muc.
        public void Update(Category category)
            => _context.Categories.Update(category);

        // Luu thay doi xuong co so du lieu.
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
