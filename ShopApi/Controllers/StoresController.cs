using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/stores")]
    public class StoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoresController(AppDbContext context)
        {
            _context = context;
        }

        // Tim danh sach cua hang theo tinh.
        [HttpGet("by-province")]
        public async Task<IActionResult> GetByProvince(string province)
        {
            var data = await _context.Stores
                .Where(x => x.Province == province)
                .ToListAsync();

            return Ok(data);
        }

        // Tim cac cua hang gan nhat theo vi do, kinh do.
        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearest(double lat, double lng)
        {
            var stores = await _context.Stores.ToListAsync();

            var result = stores
                .Select(s => new
                {
                    Store = s,
                    Distance = CalculateDistance(lat, lng, s.Latitude, s.Longitude)
                })
                .OrderBy(x => x.Distance)
                .Take(3)
                .ToList();

            return Ok(result);
        }

        // Kiem tra cua hang con ton kho theo san pham.
        [HttpGet("has-product")]
        public async Task<IActionResult> HasProduct(int productId)
        {
            var data = await _context.StoreInventories
                .Include(x => x.Store)
                .Where(x => x.ProductId == productId && x.Quantity > 0)
                .Select(x => new
                {
                    x.Store.Id,
                    x.Store.Name,
                    x.Quantity
                })
                .ToListAsync();

            return Ok(data);
        }

        // Tinh khoang cach giua hai toa do theo cong thuc Haversine.
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) *
                    Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) *
                    Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
