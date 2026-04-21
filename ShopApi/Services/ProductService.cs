using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.DTOs.Product;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        // Lay danh sach san pham, co loc theo khu vuc va khuyen mai.
        public async Task<object> GetAllAsync(ProductQuery query, string? region)
        {
            var data = _repo.GetAll();

            data = data
                .Include(x => x.ProductPromotions)
                    .ThenInclude(pp => pp.Promotion);

            data = data
                .Include(x => x.RegionPrices);

            // Tim kiem theo ten hoac SKU.
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                data = data.Where(x =>
                    x.Name.Contains(query.Keyword) ||
                    x.SKU.Contains(query.Keyword));
            }

            // Loc theo danh muc, thuong hieu va khoang gia.
            if (query.CategoryId.HasValue)
                data = data.Where(x => x.CategoryId == query.CategoryId);

            if (!string.IsNullOrEmpty(query.Brand))
                data = data.Where(x => x.Brand == query.Brand);

            if (query.MinPrice.HasValue)
                data = data.Where(x => x.Price >= query.MinPrice);

            if (query.MaxPrice.HasValue)
                data = data.Where(x => x.Price <= query.MaxPrice);

            // Sap xep theo gia hoac ngay tao.
            data = query.SortBy switch
            {
                "price_asc" => data.OrderBy(x => x.Price),
                "price_desc" => data.OrderByDescending(x => x.Price),
                _ => data.OrderByDescending(x => x.CreatedAt)
            };

            var totalCount = await data.CountAsync();

            var products = await data
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var items = products.Select(x =>
            {
                var promo = x.ProductPromotions?
                    .Select(pp => pp.Promotion)
                    .FirstOrDefault(p =>
                        p.IsActive &&
                        p.StartDate <= DateTime.Now &&
                        p.EndDate >= DateTime.Now);

                var basePrice = GetPriceByRegion(x, region);

                return new ProductResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SKU = x.SKU,
                    Brand = x.Brand,
                    Price = x.Price, // Gia goc cua san pham.

                    FinalPrice = CalculatePriceWithBase(basePrice, promo), // Gia sau khi ap dung khu vuc va khuyen mai.
                    StockQuantity = x.StockQuantity
                };
            }).ToList();

            return new
            {
                items,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                currentPage = query.PageNumber
            };
        }

        // Lay thong tin san pham theo id.
        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetAll()
                .Include(x => x.ProductPromotions)
                    .ThenInclude(pp => pp.Promotion)
                .Include(x => x.RegionPrices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return null;

            var promo = p.ProductPromotions?
                .Select(pp => pp.Promotion)
                .FirstOrDefault(x =>
                    x.IsActive &&
                    x.StartDate <= DateTime.Now &&
                    x.EndDate >= DateTime.Now);

            var basePrice = GetPriceByRegion(p, null);

            return new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Brand = p.Brand,
                Price = p.Price,
                FinalPrice = CalculatePriceWithBase(basePrice, promo),
                StockQuantity = p.StockQuantity
            };
        }

        // Tao san pham moi va kiem tra SKU trung lap.
        public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
        {
            var normalizedSku = dto.SKU?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedSku))
                throw new AppBadRequestException("SKU la bat buoc");

            var duplicateSku = await _repo.GetAll()
                .AnyAsync(x => x.SKU.ToLower() == normalizedSku.ToLower());
            if (duplicateSku)
                throw new AppConflictException("SKU da ton tai");

            var product = new Product
            {
                Name = dto.Name,
                SKU = normalizedSku,
                Brand = dto.Brand,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            var created = await GetByIdAsync(product.Id);
            if (created == null)
                throw new AppNotFoundException("Khong the tai du lieu san pham vua tao");

            return created;
        }

        // Cap nhat san pham va kiem tra SKU trung lap.
        public async Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) throw new AppNotFoundException("Khong tim thay san pham");

            var normalizedSku = dto.SKU?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedSku))
                throw new AppBadRequestException("SKU la bat buoc");

            var duplicateSku = await _repo.GetAll()
                .AnyAsync(x => x.Id != id && x.SKU.ToLower() == normalizedSku.ToLower());
            if (duplicateSku)
                throw new AppConflictException("SKU da ton tai");

            p.Name = dto.Name;
            p.SKU = normalizedSku;
            p.Price = dto.Price;
            p.Brand = dto.Brand;
            p.StockQuantity = dto.StockQuantity;
            p.Description = dto.Description;
            p.CategoryId = dto.CategoryId;
            p.IsActive = dto.IsActive;

            _repo.Update(p);
            await _repo.SaveChangesAsync();

            var updated = await GetByIdAsync(id);
            if (updated == null)
                throw new AppNotFoundException("Khong the tai du lieu san pham sau cap nhat");

            return updated;
        }

        // Xoa san pham theo id.
        public async Task<object> DeleteAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) throw new AppNotFoundException("Khong tim thay san pham");

            var deletedData = new
            {
                p.Id,
                p.Name,
                p.SKU
            };

            _repo.Delete(p);
            await _repo.SaveChangesAsync();

            return deletedData;
        }

        // Lay chi tiet san pham kem anh, thong so va san pham lien quan.
        public async Task<ProductDetailDto?> GetDetailAsync(int id, string? region)
        {
            var p = await _repo.GetAll()
                .Include(x => x.Images)
                .Include(x => x.Specifications)
                .Include(x => x.ProductPromotions)
                    .ThenInclude(pp => pp.Promotion)
                .Include(x => x.RegionPrices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return null;

            var promo = p.ProductPromotions?
                .Select(pp => pp.Promotion)
                .FirstOrDefault(x => x.IsActive);

            var basePrice = GetPriceByRegion(p, region);

            return new ProductDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                FinalPrice = CalculatePriceWithBase(basePrice, promo),

                Images = p.Images
                    .OrderBy(x => x.SortOrder)
                    .Select(x => x.ImageUrl)
                    .ToList(),

                Specifications = p.Specifications
                    .Select(s => new ProductSpecificationDto
                    {
                        Key = s.Key,
                        Value = s.Value
                    }).ToList(),

                RelatedProducts = _repo.GetAll()
                    .Where(x => x.CategoryId == p.CategoryId && x.Id != p.Id)
                    .Take(4)
                    .Select(x => new RelatedProductDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price
                    }).ToList()
            };
        }

        // Lay gia theo khu vuc; neu khong co gia khu vuc thi dung gia mac dinh.
        private decimal GetPriceByRegion(Product product, string? region)
        {
            if (string.IsNullOrEmpty(region))
                return product.Price;

            var regionPrice = product.RegionPrices?
                .FirstOrDefault(x => x.Region.ToLower() == region.ToLower());

            return regionPrice != null ? regionPrice.Price : product.Price;
        }

        // Ap dung khuyen mai len gia co so.
        private decimal CalculatePriceWithBase(decimal basePrice, Promotion? promo)
        {
            if (promo == null) return basePrice;

            if (!string.IsNullOrWhiteSpace(promo.DiscountType))
            {
                if (promo.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase))
                    return basePrice * (1 - promo.DiscountValue / 100m);

                if (promo.DiscountType.Equals("Amount", StringComparison.OrdinalIgnoreCase))
                    return basePrice - promo.DiscountValue;
            }

            return basePrice;
        }
    }
}



