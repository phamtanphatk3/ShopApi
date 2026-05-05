using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Auth;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        // Dang ky tai khoan theo contract API toi thieu.
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _service.RegisterAsync(request.Username, request.Password, request.Role);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dang ky thanh cong",
                Data = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role
                }
            });
        }

        // Xac thuc tai khoan va tra ve JWT token.
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _service.Login(request.Username, request.Password);

            if (token == null)
            {
                return Unauthorized(new ApiResponse<string?>
                {
                    Success = false,
                    Message = "Sai tai khoan hoac mat khau",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dang nhap thanh cong",
                Data = new { token }
            });
        }
    }
}

