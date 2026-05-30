using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CustomerActivity
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime ActivityDate { get; set; } = DateTime.Now;

        // تماس، جلسه، ایمیل، پیامک، بازدید، سایر
        public string ActivityType { get; set; } = "تماس";

        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // نتیجه / برآیند
        public string Outcome { get; set; } = string.Empty;

        public string OperatorName { get; set; } = "مدیر سیستم";
    }
}
