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

        public string Description { get; set; } = string.Empty;

        // محصول نهایی | مواد اولیه
        public string Category { get; set; } = "محصول نهایی";

        // درب، چهارچوب، پروفیل، پودر رنگ، ورق، ...
        public string SubCategory { get; set; } = string.Empty;

        // ابعاد فیزیکی (سانتیمتر / کیلوگرم)
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }

        public string Material { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // انواع / واریانت‌ها (JSON)
        public string VariantsJson { get; set; } = "[]";

        // تصاویر محصول (JSON آرایه Base64)
        public string ImagesJson { get; set; } = "[]";

        // قیمت‌گذاری
        public decimal PurchasePrice { get; set; }
        public decimal Price { get; set; }        // قیمت فروش / روز
        public decimal MinSalePrice { get; set; } // حداقل قیمت قابل فروش

        // انبار
        public double StockQuantity { get; set; }
        public string Unit { get; set; } = "عدد"; // عدد، کیلوگرم، متر
        public double MinStockAlert { get; set; } = 5;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
