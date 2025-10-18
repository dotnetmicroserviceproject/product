using common.Entities;

namespace Product.Service.Entities
{
    public class Items: EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } 
    }
}
