using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories
                .Where(x => x.IsActive)
                .ToListAsync();

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        public async Task<bool> HasProductsAsync(int categoryId)
            => await _context.Products
                .AnyAsync(x => x.CategoryId == categoryId && x.IsActive);

        public async Task AddAsync(Category category)
            => await _context.Categories.AddAsync(category);

        public void Update(Category category)
            => _context.Categories.Update(category);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
