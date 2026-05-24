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

        // وضعیت: ثبت اولیه -> آماده تحویل -> در حال ارسال -> تحویل شده
        public string Status { get; set; } = "ثبت اولیه";

        public int? CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public DateTime? DispatchDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        // اطلاعات لجستیک و باربری
        public string DriverName { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;

        public string ItemsJson { get; set; } = "[]";

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalAmount { get; set; }

        // اطلاعات تسویه حساب
        public string PaymentMethod { get; set; } = "نقدی"; // نقدی، چکی، ترکیبی
        public decimal CashAmount { get; set; }
        public decimal CheckAmount { get; set; }
        public string CheckNumber { get; set; } = string.Empty;   // شماره صیاد 16 رقمی
        public string CheckBankName { get; set; } = string.Empty;
        public DateTime? CheckDueDate { get; set; }
        public string CheckDetails { get; set; } = string.Empty;  // خلاصه اطلاعات چک برای چاپ

        public string Notes { get; set; } = string.Empty;
    }
}
