using ShopApi.DTOs.Product;

namespace ShopApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<object> GetAllAsync(ProductQuery query, string? region);
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(int id, ProductUpdateDto dto);
        Task DeleteAsync(int id);
        Task<ProductDetailDto?> GetDetailAsync(int id, string? region);
    }
}
