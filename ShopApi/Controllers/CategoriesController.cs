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

        // Lay danh sach danh muc.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(new ApiResponse<List<CategoryResponseDto>>
            {
                Success = true,
                Message = "Lay danh sach thanh cong",
                Data = data
            });
        }

        // Lay chi tiet danh muc theo id.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string?>
                {
                    Success = false,
                    Message = "Khong tim thay",
                    Data = null
                });

            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Tao danh muc moi (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var data = await _service.CreateAsync(dto);

            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = data
            });
        }

        // Cap nhat danh muc theo id (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        // Xoa mem danh muc theo id (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("{id}")]
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
    }
}
