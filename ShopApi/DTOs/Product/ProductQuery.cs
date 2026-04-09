namespace ShopApi.DTOs.Product
{
    public class ProductQuery
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string? Brand { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; } // price_asc, price_desc

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
