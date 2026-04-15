namespace ShopApi.Models
{
    public class StoreInventory
    {
        public int Id { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
