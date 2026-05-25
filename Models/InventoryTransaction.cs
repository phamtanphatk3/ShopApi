namespace ShopApi.Models
{
    // Mo hinh du lieu InventoryTransaction.
    public class InventoryTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; } // + hoặc -

        public string Type { get; set; } = string.Empty; // "IMPORT" | "EXPORT"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Product Product { get; set; } = null!;
    }
}

