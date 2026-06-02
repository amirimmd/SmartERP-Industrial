using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }

        // تاریخ شمسی به فرمت "yyyy/MM/dd"
        public string JalaliDate { get; set; } = string.Empty;

        // تاریخ میلادی واقعی برای مرتب‌سازی و مقایسه
        public DateTime EventDate { get; set; } = DateTime.Today;

        public string Title { get; set; } = string.Empty;

        // چک | مالی | تولید | تحویل | یادآوری | جلسه | ضرب_الاجل
        public string EventType { get; set; } = "یادآوری";

        public string Description { get; set; } = string.Empty;

        public string AuthorRole { get; set; } = "عمومی";

        public string AuthorName { get; set; } = "مدیر سیستم";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ارجاع به فاکتور یا سفارش مرتبط (اختیاری)
        public int? InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        // فیلدهای اختصاصی چک
        public decimal? CheckAmount { get; set; }
        public string CheckNumber { get; set; } = string.Empty;
    }
}
