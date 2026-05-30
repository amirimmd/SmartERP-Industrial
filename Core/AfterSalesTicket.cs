using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class AfterSalesTicket
    {
        [Key]
        public int Id { get; set; }

        public int? InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // خرابی، تغییر، بازدید دوره‌ای، شکایت، سایر
        public string IssueType { get; set; } = "خرابی";

        public string Description { get; set; } = string.Empty;

        // باز، در بررسی، رفع شده، بسته
        public string Status { get; set; } = "باز";

        public int? AssignedTechnicianId { get; set; }
        public string AssignedTechnicianName { get; set; } = string.Empty;

        public DateTime ReportedDate { get; set; } = DateTime.Now;
        public DateTime? ResolvedDate { get; set; }
        public string Resolution { get; set; } = string.Empty;
    }
}
