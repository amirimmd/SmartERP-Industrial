using System;
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
        public string Email { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string CustomerType { get; set; } = "خریدار عادی"; // همکار، پیمانکار، دولتی، عمده‌فروش

        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime? LastPurchaseDate { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalInvoiceCount { get; set; }
    }
}