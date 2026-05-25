using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    [Authorize(Roles = "Customer")]
    public class WishlistController : ControllerBase
    {
        private readonly WishlistService _service;

        public WishlistController(WishlistService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWishlist()
        {
            var data = await _service.GetMyWishlistAsync();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            var data = await _service.AddAsync(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Them vao danh sach yeu thich thanh cong",
                Data = data
            });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var data = await _service.RemoveAsync(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xoa khoi danh sach yeu thich thanh cong",
                Data = data
            });
        }
    }
}
