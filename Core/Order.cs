using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        // ارتباط با فاکتور مرجع
        public int? InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime ExpectedDelivery { get; set; } = DateTime.Now.AddDays(7);

        // وضعیت: در انتظار بررسی، در حال ساخت، آماده تحویل
        public string Status { get; set; } = "در انتظار بررسی";

        // اقلام JSON (منتقل‌شده از فاکتور یا متن آزاد)
        public string ItemsJson { get; set; } = "[]";

        public decimal TotalAmount { get; set; }

        // مشخصات فنی اضافی برای چاپ دستور کار
        public string TechnicalSpecs { get; set; } = string.Empty;

        // یادداشت مدیر تولید
        public string ProductionManagerNotes { get; set; } = string.Empty;

        // اولویت تولید: فوری | عادی | کم‌اولویت
        public string ProductionPriority { get; set; } = "عادی";

        // تاریخ شروع واقعی تولید
        public DateTime? ProductionStartDate { get; set; }

        // تاریخ تکمیل تولید
        public DateTime? CompletedAt { get; set; }

        // درصد پیشرفت محاسبه‌شده بر اساس وضعیت
        public int GetCompletionPercentage() => Status switch
        {
            "آماده تحویل" => 100,
            "در حال ساخت" or "در حال تولید" => 50,
            _ => 0  // ثبت شده / در انتظار بررسی
        };

        // مرحله گردش‌کار (۱=ثبت شده، ۲=در حال تولید، ۳=آماده تحویل)
        public int GetWorkflowStage() => Status switch
        {
            "آماده تحویل" => 3,
            "در حال ساخت" or "در حال تولید" => 2,
            _ => 1
        };
    }
}
