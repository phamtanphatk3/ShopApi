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

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug
            }).ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug
            };
        }

        public async Task CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Slug = BuildSlug(dto.Slug, dto.Name),
                ParentCategoryId = dto.ParentCategoryId
            };

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new Exception("Category not found");

            c.Name = dto.Name;
            c.Slug = BuildSlug(dto.Slug, dto.Name);
            c.IsActive = dto.IsActive;

            _repo.Update(c);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new Exception("Category not found");

            var hasProducts = await _repo.HasProductsAsync(id);
            if (hasProducts)
                throw new Exception("Cannot delete category because it has products");

            c.IsActive = false;
            _repo.Update(c);
            await _repo.SaveChangesAsync();
        }

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
