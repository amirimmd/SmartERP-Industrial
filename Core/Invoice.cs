using System;
using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        
        public string InvoiceNumber { get; set; } = string.Empty;
        
        // نوع: پیش‌فاکتور، فاکتور رسمی، حواله خروج
        public string InvoiceType { get; set; } = "پیش‌فاکتور"; 
        
        // وضعیت: ثبت اولیه -> در حال تولید -> آماده تحویل -> در حال ارسال -> تحویل شده
        public string Status { get; set; } = "ثبت اولیه"; 
        
        // اطلاعات کامل مشتری در این سند
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        
        // اطلاعات لجستیک و باربری
        public string DriverName { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        
        public string ItemsJson { get; set; } = "[]"; 
        
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalAmount { get; set; }
        
        public string Notes { get; set; } = string.Empty;
    }
}
