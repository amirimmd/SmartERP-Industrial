using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        // پیش‌فاکتور | فاکتور رسمی | حواله خروج
        public string InvoiceType { get; set; } = "پیش‌فاکتور";

        // وضعیت ۶ مرحله‌ای:
        // ثبت سفارش → در حال ساخت → آماده ارسال → بارگیری و تحویل → نصب و راه‌اندازی → پشتیبانی و خدمات
        public string Status { get; set; } = "ثبت سفارش";

        // مرحله جاری (1-6) برای نمایش سریع‌تر
        public int CurrentStep { get; set; } = 1;

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public DateTime? DispatchDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        // یادداشت هر مرحله
        public string Step1Notes { get; set; } = string.Empty;
        public string Step2Notes { get; set; } = string.Empty;
        public string Step3Notes { get; set; } = string.Empty;
        public string Step4Notes { get; set; } = string.Empty;
        public string Step5Notes { get; set; } = string.Empty;
        public string Step6Notes { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // مرحله ۳: لجستیک و ارسال
        public string DriverName { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        public bool ReceivedInGoodCondition { get; set; } = false;
        public string ReceiverName { get; set; } = string.Empty;

        // اقلام فاکتور (JSON)
        public string ItemsJson { get; set; } = "[]";

        // مالی
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalAmount { get; set; }

        // تسویه‌حساب
        public string PaymentMethod { get; set; } = "نقدی"; // نقدی، چکی، ترکیبی
        public decimal CashAmount { get; set; }
        public decimal CheckAmount { get; set; }
        public string CheckNumber { get; set; } = string.Empty;
        public string CheckBankName { get; set; } = string.Empty;
        public DateTime? CheckDueDate { get; set; }
        public string CheckDetails { get; set; } = string.Empty;

        // مرحله ۲: تولید
        public bool NeedsProduction { get; set; } = false;
        public bool AutoCreateProductionOrder { get; set; } = false;
        public string ProductionNotes { get; set; } = string.Empty;
        public DateTime? ProductionDate { get; set; }
        public DateTime? ProductionDeadline { get; set; }
        public bool IsUrgentProduction { get; set; } = false;

        // مرحله ۵: نصب و راه‌اندازی
        public int? InstallerPersonnelId { get; set; }
        public string InstallerName { get; set; } = string.Empty;
        public DateTime? InstallationDateTime { get; set; }
        public string InstallationNotes { get; set; } = string.Empty;

        // مرحله ۶: خدمات پس از فروش
        public int WarrantyMonths { get; set; } = 0;
        public string AfterSaleNotes { get; set; } = string.Empty;
    }
}
