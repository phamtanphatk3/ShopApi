using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Common;
using ShopApi.Data;
using ShopApi.DTOs.Product;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/region-prices")]
    [Authorize(Roles = "Admin,Staff")]
    public class ProductRegionPricesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductRegionPricesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new Exception("Product not found");

            var data = await _context.ProductRegionPrices
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.Region)
                .Select(x => new
                {
                    x.Id,
                    x.Region,
                    x.Price
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(int productId, [FromBody] ProductRegionPriceCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Region))
                throw new Exception("Region is required");
            if (dto.Price <= 0)
                throw new Exception("Price must be greater than 0");

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new Exception("Product not found");

            var region = dto.Region.Trim();
            var existed = await _context.ProductRegionPrices.AnyAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (existed)
                throw new Exception("Region price already exists for this product");

            var item = new ProductRegionPrice
            {
                ProductId = productId,
                Region = region,
                Price = dto.Price
            };

            _context.ProductRegionPrices.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Created",
                Data = new { item.Id, item.ProductId, item.Region, item.Price }
            });
        }

        [HttpPut("{region}")]
        public async Task<IActionResult> Update(int productId, string region, [FromBody] ProductRegionPriceUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new Exception("Region is required");
            if (dto.Price <= 0)
                throw new Exception("Price must be greater than 0");

            var item = await _context.ProductRegionPrices.FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (item == null)
                throw new Exception("Region price not found");

            item.Price = dto.Price;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Updated",
                Data = new { item.Id, item.ProductId, item.Region, item.Price }
            });
        }

        [HttpDelete("{region}")]
        public async Task<IActionResult> Delete(int productId, string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new Exception("Region is required");

            var item = await _context.ProductRegionPrices.FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (item == null)
                throw new Exception("Region price not found");

            _context.ProductRegionPrices.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string?>
            {
                Success = true,
                Message = "Deleted",
                Data = null
            });
        }
    }
}
