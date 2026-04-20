using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
    // Hop dong truy cap du lieu san pham.
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task SaveChangesAsync();
    }
}
