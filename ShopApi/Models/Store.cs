namespace ShopApi.Models
{
    // Mo hinh du lieu Store.
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<StoreInventory> Inventories { get; set; } = new();
    }
}

