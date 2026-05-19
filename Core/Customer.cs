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
        
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        
        public string CustomerType { get; set; } = "خریدار عادی"; // همکار، پیمانکار، دولتی
    }
}