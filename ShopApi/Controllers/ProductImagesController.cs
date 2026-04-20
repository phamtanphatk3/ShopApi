using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.ProductImage;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly ProductImageService _service;

        public ProductImagesController(ProductImageService service)
        {
            _service = service;
        }

        // Upload anh cho san pham va tra ve danh sach anh sau khi cap nhat.
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Upload(int productId, [FromForm] UploadImageDto dto)
        {
            await _service.UploadAsync(productId, dto);
            var data = await _service.GetImagesAsync(productId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Upload thanh cong",
                Data = data
            });
        }

        // Lay danh sach anh cua san pham.
        [HttpGet]
        public async Task<IActionResult> GetImages(int productId)
        {
            var data = await _service.GetImagesAsync(productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }
    }
}
