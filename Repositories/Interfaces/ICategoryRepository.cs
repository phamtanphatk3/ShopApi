using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
    // Hop dong truy cap du lieu danh muc.
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> HasProductsAsync(int categoryId);
        Task AddAsync(Category category);
        void Update(Category category);
        Task SaveChangesAsync();
    }
}
