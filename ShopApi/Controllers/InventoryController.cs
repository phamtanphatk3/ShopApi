using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Inventory;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/inventory")]
    [Authorize(Roles = "Admin,Staff")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        // Nhap kho cho san pham va tra ve ton kho moi.
        [HttpPost("import")]
        public async Task<IActionResult> Import(int productId, InventoryDto dto)
        {
            await _service.ImportAsync(productId, dto.Quantity);
            var data = await _service.GetStockAsync(productId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Nhap kho thanh cong",
                Data = data
            });
        }

        // Xuat kho cho san pham va tra ve ton kho moi.
        [HttpPost("export")]
        public async Task<IActionResult> Export(int productId, InventoryDto dto)
        {
            await _service.ExportAsync(productId, dto.Quantity);
            var data = await _service.GetStockAsync(productId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xuat kho thanh cong",
                Data = data
            });
        }

        // Lay lich su giao dich nhap/xuat kho cua san pham.
        [HttpGet("history")]
        public async Task<IActionResult> History(int productId)
        {
            var data = await _service.GetHistory(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}

