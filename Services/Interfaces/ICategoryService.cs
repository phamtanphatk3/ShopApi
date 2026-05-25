using ShopApi.DTOs.Category;

namespace ShopApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto?> GetByIdAsync(int id);
        Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
        Task<CategoryResponseDto> UpdateAsync(int id, CategoryUpdateDto dto);
        Task<object> DeleteAsync(int id);
    }
}
