using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu ProductRegionPriceUpdateDto.
    public class ProductRegionPriceUpdateDto
    {
        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal Price { get; set; }
    }
}

