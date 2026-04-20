namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu ProductUpdateDto.
    public class ProductUpdateDto : ProductCreateDto
    {
        public bool IsActive { get; set; }
    }
}

