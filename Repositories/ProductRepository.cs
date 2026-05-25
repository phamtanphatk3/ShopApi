using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;


namespace ShopApi.Repositories
{
    // Repository xu ly truy cap du lieu san pham.
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lay query san pham de service tiep tuc loc/sap xep/phan trang.
        public IQueryable<Product> GetAll()
            => _context.Products.AsQueryable();

        // Lay san pham theo id kem anh, thong so va khuyen mai.
        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products
        .Include(x => x.Images)
        .Include(x => x.Specifications)
        .Include(x => x.ProductPromotions)
            .ThenInclude(pp => pp.Promotion)
        .FirstOrDefaultAsync(x => x.Id == id);

        // Them moi san pham.
        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        // Cap nhat san pham.
        public void Update(Product product)
            => _context.Products.Update(product);

        // Xoa san pham.
        public void Delete(Product product)
            => _context.Products.Remove(product);

        // Luu thay doi xuong co so du lieu.
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
