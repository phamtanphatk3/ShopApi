using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;


namespace ShopApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetAll()
            => _context.Products.AsQueryable();
        //.Include(x => x.ProductPromotions)
        //.ThenInclude(pp => pp.Promotion);

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products
        .Include(x => x.Images)
        .Include(x => x.Specifications)
        .Include(x => x.ProductPromotions)
            .ThenInclude(pp => pp.Promotion)
        .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        public void Update(Product product)
            => _context.Products.Update(product);

        public void Delete(Product product)
            => _context.Products.Remove(product);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
