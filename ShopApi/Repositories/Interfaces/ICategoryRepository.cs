using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
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
