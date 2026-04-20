using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Product;
using ShopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // Lay danh sach san pham theo bo loc, sap xep, phan trang va khu vuc.
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
            [FromQuery] ProductQuery query,
            [FromQuery] string? region)
        {
            var data = await _service.GetAllAsync(query, region);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        // Lay chi tiet san pham theo id va khu vuc.
        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(
            int id,
            [FromQuery] string? region)
        {
            var data = await _service.GetDetailAsync(id, region);
            if (data == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Product not found"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        // Tao san pham moi (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<ProductResponseDto>
            {
                Success = true,
                Message = "Created",
                Data = data
            });
        }

        // Cap nhat san pham theo id (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<ProductResponseDto>
            {
                Success = true,
                Message = "Updated",
                Data = data
            });
        }

        // Xoa san pham theo id (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Deleted",
                Data = data
            });
        }
    }
}
