using ShopApi.DTOs.Product;

namespace ShopApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<object> GetAllAsync(ProductQuery query, string? region);
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);
        Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto);
        Task<object> DeleteAsync(int id);
        Task<ProductDetailDto?> GetDetailAsync(int id, string? region);
    }
}
