using Microsoft.EntityFrameworkCore;
using ShopApi.Data;

namespace ShopApi.Services
{
    // Xu ly nghiep vu tim cua hang theo tinh, theo toa do va ton kho theo san pham.
    public class StoreService
    {
        private readonly AppDbContext _context;

        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        // Lay danh sach cua hang theo ten tinh/thanh.
        public async Task<object> GetByProvince(string province)
        {
            return await _context.Stores
                .Where(x => x.Province == province)
                .ToListAsync();
        }

        // Lay 3 cua hang gan nhat tu vi tri truyen vao.
        public async Task<object> GetNearest(double lat, double lng)
        {
            var stores = await _context.Stores.ToListAsync();

            return stores
                .Select(s => new
                {
                    Store = s,
                    Distance = CalculateDistance(lat, lng, s.Latitude, s.Longitude)
                })
                .OrderBy(x => x.Distance)
                .Take(3)
                .ToList();
        }

        // Kiem tra cua hang nao con hang cua san pham.
        public async Task<object> HasProduct(int productId)
        {
            return await _context.StoreInventories
                .Include(x => x.Store)
                .Where(x => x.ProductId == productId && x.Quantity > 0)
                .Select(x => new
                {
                    x.Store.Id,
                    x.Store.Name,
                    x.Quantity
                })
                .ToListAsync();
        }

        // Tinh khoang cach Haversine.
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var r = 6371d;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) *
                    Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) *
                    Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return r * c;
        }
    }
}


