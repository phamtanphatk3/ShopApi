using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Cart;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        // ================= ADD TO CART =================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
        {
            await _service.AddToCart(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Added to cart"
            });
        }

        // ================= GET CART =================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetCartDetail();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        // ================= UPDATE =================
        [HttpPut]
        public async Task<IActionResult> Update(int itemId, int quantity)
        {
            await _service.UpdateItem(itemId, quantity);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Updated"
            });
        }

        // ================= DELETE =================
        [HttpDelete]
        public async Task<IActionResult> Remove(int itemId)
        {
            await _service.RemoveItem(itemId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Removed"
            });
        }
    }
}
