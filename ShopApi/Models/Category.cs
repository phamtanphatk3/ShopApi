namespace ShopApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;

        public Category? ParentCategory { get; set; }
        public List<Category> Children { get; set; }
    }
}
