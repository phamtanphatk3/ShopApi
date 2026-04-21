using Microsoft.AspNetCore.Mvc;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/stores")]
    public class StoresController : ControllerBase
    {
        private readonly StoreService _service;

        public StoresController(StoreService service)
        {
            _service = service;
        }

        // Tim danh sach cua hang theo tinh.
        [HttpGet("by-province")]
        public async Task<IActionResult> GetByProvince(string province)
        {
            var data = await _service.GetByProvince(province);
            return Ok(data);
        }

        // Tim cac cua hang gan nhat theo vi do, kinh do.
        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearest(double lat, double lng)
        {
            var result = await _service.GetNearest(lat, lng);
            return Ok(result);
        }

        // Kiem tra cua hang con ton kho theo san pham.
        [HttpGet("has-product")]
        public async Task<IActionResult> HasProduct(int productId)
        {
            var data = await _service.HasProduct(productId);
            return Ok(data);
        }
    }
}

