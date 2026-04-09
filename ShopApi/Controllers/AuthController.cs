using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShopApi.DTOs.Auuth;
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _service.Login(request.Username, request.Password);

            if (token == null)
                return Unauthorized("Sai tài khoản hoặc mật khẩu");

            return Ok(new { token });
        }
    }
}