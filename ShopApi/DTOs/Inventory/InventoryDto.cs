using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Inventory
{
    // DTO trao doi du lieu InventoryDto.
    public class InventoryDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}

