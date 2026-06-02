using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int? InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime ExpectedDelivery { get; set; } = DateTime.Now.AddDays(7);

        // وضعیت: در انتظار بررسی، در حال ساخت، آماده تحویل
        public string Status { get; set; } = "در انتظار بررسی";

        public string ItemsJson { get; set; } = "[]";

        public decimal TotalAmount { get; set; }

        public string TechnicalSpecs { get; set; } = string.Empty;

        public string ProductionManagerNotes { get; set; } = string.Empty;

        // اولویت تولید: فوری | عادی | کم‌اولویت
        public string ProductionPriority { get; set; } = "عادی";

        public DateTime? ProductionStartDate { get; set; }

        // تاریخ شروع واقعی تولید (مرحله ۲)
        public DateTime? ProductionStartedAt { get; set; }

        // تاریخ آماده‌شدن تحویل (مرحله ۳)
        public DateTime? ReadyAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int GetCompletionPercentage() => Status switch
        {
            "آماده تحویل" => 100,
            "در حال ساخت" or "در حال تولید" => 50,
            _ => 0
        };

        public int GetWorkflowStage() => Status switch
        {
            "آماده تحویل" => 3,
            "در حال ساخت" or "در حال تولید" => 2,
            _ => 1
        };
    }
}
