using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }

        // تاریخ شمسی به فرمت "yyyy/MM/dd" — مثال: 1404/02/15
        public string JalaliDate { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        // چک | گزارش_مالی | یادآوری | جلسه | ضرب_الاجل
        public string EventType { get; set; } = "یادآوری";

        public string Description { get; set; } = string.Empty;

        // حسابدار | مدیر | عمومی
        public string AuthorRole { get; set; } = "عمومی";

        public string AuthorName { get; set; } = "مدیر سیستم";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // فیلدهای اختصاصی چک
        public decimal? CheckAmount { get; set; }
        public string CheckNumber { get; set; } = string.Empty;
    }
}
