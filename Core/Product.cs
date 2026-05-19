using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
        
        public double StockQuantity { get; set; }
        
        public string Unit { get; set; } = "عدد";
    }
}