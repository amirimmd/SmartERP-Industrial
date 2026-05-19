using System;
using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public string OrderNumber { get; set; } = string.Empty;
        
        public string CustomerName { get; set; } = string.Empty;
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        public DateTime ExpectedDelivery { get; set; } = DateTime.Now.AddDays(7);
        
        // وضعیت ارتباط با تولید: بررسی، در حال ساخت، آماده تحویل
        public string Status { get; set; } = "در انتظار بررسی"; 
        
        public string ItemsJson { get; set; } = "[]"; 
        
        public decimal TotalAmount { get; set; }
    }
}