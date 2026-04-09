using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Category;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(new ApiResponse<List<CategoryResponseDto>>
            {
                Success = true,
                Message = "L?y danh sách thành công",
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Không tìm th?y",
                    Data = null
                });

            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            });
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            await _service.CreateAsync(dto);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "T?o thành công",
                Data = null
            });
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "C?p nh?t thành công",
                Data = null
            });
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Xóa thành công",
                Data = null
            });
        }
    }
}
