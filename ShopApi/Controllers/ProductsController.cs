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

        // ================= GET LIST =================
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

        // ================= GET DETAIL =================
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

        // ================= CREATE =================
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Created"
            });
        }

        // ================= UPDATE =================
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Updated"
            });
        }

        // ================= DELETE =================
        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Deleted"
            });
        }
    }
}
