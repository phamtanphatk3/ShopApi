using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Store;
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

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Khong tim thay cua hang" });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] StoreCreateDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = data
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] StoreUpdateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xoa thanh cong",
                Data = data
            });
        }

        // Tim danh sach cua hang theo tinh.
        [HttpGet("by-province")]
        public async Task<IActionResult> GetByProvince(string province)
        {
            var data = await _service.GetByProvince(province);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Tim cac cua hang gan nhat theo vi do, kinh do.
        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearest(double lat, double lng)
        {
            var result = await _service.GetNearest(lat, lng);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = result
            });
        }

        // Kiem tra cua hang con ton kho theo san pham.
        [HttpGet("has-product")]
        public async Task<IActionResult> HasProduct(int productId)
        {
            var data = await _service.HasProduct(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}
