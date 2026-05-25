using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Store;
using ShopApi.Models;

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

        public async Task<List<StoreResponseDto>> GetAllAsync()
        {
            return await _context.Stores
                .OrderByDescending(x => x.Id)
                .Select(x => new StoreResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Province = x.Province,
                    District = x.District,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
                .ToListAsync();
        }

        public async Task<StoreResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Stores
                .Where(x => x.Id == id)
                .Select(x => new StoreResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Province = x.Province,
                    District = x.District,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StoreResponseDto> CreateAsync(StoreCreateDto dto)
        {
            var store = new Store
            {
                Name = dto.Name,
                Province = dto.Province,
                District = dto.District,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Province = store.Province,
                District = store.District,
                Address = store.Address,
                Latitude = store.Latitude,
                Longitude = store.Longitude
            };
        }

        public async Task<StoreResponseDto> UpdateAsync(int id, StoreUpdateDto dto)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
            if (store == null)
                throw new AppNotFoundException("Khong tim thay cua hang");

            store.Name = dto.Name;
            store.Province = dto.Province;
            store.District = dto.District;
            store.Address = dto.Address;
            store.Latitude = dto.Latitude;
            store.Longitude = dto.Longitude;

            await _context.SaveChangesAsync();

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Province = store.Province,
                District = store.District,
                Address = store.Address,
                Latitude = store.Latitude,
                Longitude = store.Longitude
            };
        }

        public async Task<object> DeleteAsync(int id)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
            if (store == null)
                throw new AppNotFoundException("Khong tim thay cua hang");

            var inventories = await _context.StoreInventories.Where(x => x.StoreId == id).ToListAsync();
            _context.StoreInventories.RemoveRange(inventories);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return new { store.Id };
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
