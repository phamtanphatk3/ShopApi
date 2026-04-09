namespace ShopApi.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPrimary { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
