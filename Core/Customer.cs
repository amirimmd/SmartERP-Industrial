using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // خریدار عادی، همکار، پیمانکار، دولتی، عمده‌فروش
        public string CustomerType { get; set; } = "خریدار عادی";

        // منبع جذب مشتری: اینستاگرام، معرفی، وب‌سایت، تبلیغات، نمایشگاه، سایر
        public string LeadSource { get; set; } = string.Empty;

        // نام معرف (برای «معرفی»)
        public string ReferredBy { get; set; } = string.Empty;

        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime? LastPurchaseDate { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalInvoiceCount { get; set; }
    }
}
