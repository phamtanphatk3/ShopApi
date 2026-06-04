using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Auth;
using ShopApi.Services;
using System.Security.Claims;

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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _service.RegisterAsync(
                request.Username,
                request.Password,
                request.Role,
                request.Email,
                request.Phone,
                request.Address);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dang ky thanh cong",
                Data = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role,
                    email = user.Email,
                    phone = user.Phone,
                    address = user.Address
                }
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _service.LoginAsync(request.Username, request.Password);

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
                Data = new
                {
                    token = token.AccessToken,
                    refreshToken = token.RefreshToken,
                    refreshTokenExpiresAt = token.RefreshTokenExpiresAt,
                    user = token.User
                }
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var session = await _service.RefreshAsync(request.RefreshToken);
            if (session == null)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Refresh token khong hop le hoac da het han"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Lam moi token thanh cong",
                Data = new
                {
                    token = session.AccessToken,
                    refreshToken = session.RefreshToken,
                    refreshTokenExpiresAt = session.RefreshTokenExpiresAt,
                    user = session.User
                }
            });
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var success = await _service.LogoutAsync(request.RefreshToken);
            if (!success)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Refresh token khong hop le hoac da het han"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dang xuat thanh cong",
                Data = new { revoked = true }
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Chua dang nhap" });

            var data = await _service.GetProfileAsync(int.Parse(userIdClaim.Value));
            if (data == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Khong tim thay nguoi dung" });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpPut("me/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Chua dang nhap" });

            var data = await _service.ChangePasswordAsync(int.Parse(userIdClaim.Value), dto.CurrentPassword, dto.NewPassword);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat mat khau thanh cong",
                Data = data
            });
        }
    }
}
