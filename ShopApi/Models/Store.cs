namespace ShopApi.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; } // HCM, HN
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<StoreInventory> Inventories { get; set; }
    }
}
