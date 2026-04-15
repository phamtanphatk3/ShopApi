using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("import")]
        public async Task<IActionResult> Import(int productId, InventoryDto dto)
        {
            await _service.ImportAsync(productId, dto.Quantity);
            return Ok("Nhap kho thanh cong");
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export(int productId, InventoryDto dto)
        {
            await _service.ExportAsync(productId, dto.Quantity);
            return Ok("Xuat kho thanh cong");
        }

        [HttpGet("history")]
        public async Task<IActionResult> History(int productId)
        {
            var data = await _service.GetHistory(productId);
            return Ok(data);
        }
    }
}
