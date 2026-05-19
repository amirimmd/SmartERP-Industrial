using System;
using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        
        public string InvoiceNumber { get; set; } = string.Empty;
        
        public string CustomerName { get; set; } = string.Empty;
        
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        
        // ذخیره محصولات فاکتور به صورت ساختاریافته و فوق‌سبک JSON
        public string ItemsJson { get; set; } = "[]"; 
        
        public decimal TotalAmount { get; set; }
        
        public decimal Discount { get; set; }
        
        public decimal Tax { get; set; }
        
        public decimal FinalAmount { get; set; }
    }
}