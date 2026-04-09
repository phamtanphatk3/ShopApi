using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // ? Upload ?nh
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Upload(int productId, [FromForm] UploadImageDto dto)
        {
            await _service.UploadAsync(productId, dto);
            return Ok(new { message = "Upload thành công" });
        }

        // ? L?y ?nh (d? test)
        [HttpGet]
        public async Task<IActionResult> GetImages(int productId)
        {
            var data = await _service.GetImagesAsync(productId);
            return Ok(data);
        }
    }

}
