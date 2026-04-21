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

        // Them san pham vao gio hang cua khach hang hien tai va tra ve gio hang sau khi cap nhat.
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
        {
            await _service.AddToCart(dto);
            var data = await _service.GetCartDetail();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Them vao gio thanh cong",
                Data = data
            });
        }

        // Lay chi tiet gio hang cua khach hang hien tai.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetCartDetail();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Cap nhat so luong cho mot item trong gio hang cua khach hang hien tai.
        [HttpPut]
        public async Task<IActionResult> Update(int itemId, int quantity)
        {
            await _service.UpdateItem(itemId, quantity);
            var data = await _service.GetCartDetail();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        // Xoa mot item khoi gio hang cua khach hang hien tai.
        [HttpDelete]
        public async Task<IActionResult> Remove(int itemId)
        {
            await _service.RemoveItem(itemId);
            var data = await _service.GetCartDetail();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xoa khoi gio thanh cong",
                Data = data
            });
        }
    }
}

