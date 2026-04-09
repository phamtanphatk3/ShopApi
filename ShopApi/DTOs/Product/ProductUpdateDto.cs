namespace ShopApi.DTOs.Product
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public bool IsActive { get; set; }
    }
}
