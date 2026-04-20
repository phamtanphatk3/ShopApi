using ShopApi.DTOs.Category;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;
using System.Text.RegularExpressions;

namespace ShopApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        // Lay danh sach danh muc va map sang DTO.
        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive
            }).ToList();
        }

        // Lay chi tiet danh muc theo id.
        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive
            };
        }

        // Tao danh muc moi va tao slug tu ten/slug dau vao.
        public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Slug = BuildSlug(dto.Slug, dto.Name),
                ParentCategoryId = dto.ParentCategoryId
            };

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };
        }

        // Cap nhat thong tin danh muc theo id.
        public async Task<CategoryResponseDto> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new Exception("Category not found");

            c.Name = dto.Name;
            c.Slug = BuildSlug(dto.Slug, dto.Name);
            c.IsActive = dto.IsActive;

            _repo.Update(c);
            await _repo.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive
            };
        }

        // Xoa mem danh muc neu khong ton tai san pham.
        public async Task<object> DeleteAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new Exception("Category not found");

            var hasProducts = await _repo.HasProductsAsync(id);
            if (hasProducts)
                throw new Exception("Cannot delete category because it has products");

            c.IsActive = false;
            _repo.Update(c);
            await _repo.SaveChangesAsync();

            return new
            {
                c.Id,
                c.Name,
                c.Slug,
                c.ParentCategoryId,
                c.IsActive
            };
        }

        // Chuan hoa slug ve dang lowercase-kebab-case.
        private static string BuildSlug(string? slug, string name)
        {
            var source = string.IsNullOrWhiteSpace(slug) ? name : slug;
            var normalized = source.Trim().ToLowerInvariant();
            normalized = normalized.Replace("đ", "d");
            normalized = Regex.Replace(normalized, @"\s+", "-");
            normalized = Regex.Replace(normalized, @"[^a-z0-9\-]", "");
            normalized = Regex.Replace(normalized, @"-+", "-").Trim('-');
            return normalized;
        }
    }
}
