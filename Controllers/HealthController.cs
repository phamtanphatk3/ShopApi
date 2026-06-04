using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Common;
using ShopApi.Data;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/health")]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var canConnect = await _context.Database.CanConnectAsync();
            var payload = new
            {
                status = canConnect ? "healthy" : "unhealthy",
                database = _context.Database.ProviderName,
                timestamp = DateTime.UtcNow
            };

            if (!canConnect)
            {
                return StatusCode(503, new ApiResponse<object>
                {
                    Success = false,
                    Message = "He thong database chua san sang",
                    Data = payload
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "He thong san sang",
                Data = payload
            });
        }
    }
}
