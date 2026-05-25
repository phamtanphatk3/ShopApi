using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Product;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/region-prices")]
    [Authorize(Roles = "Admin,Staff")]
    public class ProductRegionPricesController : ControllerBase
    {
        private readonly ProductRegionPriceService _service;

        public ProductRegionPricesController(ProductRegionPriceService service)
        {
            _service = service;
        }

        // Lay danh sach gia theo khu vuc cua mot san pham.
        [HttpGet]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var data = await _service.GetByProduct(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Tao gia khu vuc moi cho san pham.
        [HttpPost]
        public async Task<IActionResult> Create(int productId, [FromBody] ProductRegionPriceCreateDto dto)
        {
            var data = await _service.Create(productId, dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = data
            });
        }

        // Cap nhat gia khu vuc cua san pham theo ten khu vuc.
        [HttpPut("{region}")]
        public async Task<IActionResult> Update(int productId, string region, [FromBody] ProductRegionPriceUpdateDto dto)
        {
            var data = await _service.Update(productId, region, dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        // Xoa gia khu vuc cua san pham theo ten khu vuc.
        [HttpDelete("{region}")]
        public async Task<IActionResult> Delete(int productId, string region)
        {
            var data = await _service.Delete(productId, region);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xoa thanh cong",
                Data = data
            });
        }
    }
}

