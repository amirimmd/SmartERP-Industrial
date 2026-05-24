using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CompanySetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Slogan { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public decimal DefaultTaxRate { get; set; } = 0.09m;

        // اطلاعات حساب بانکی
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ShabaNumber { get; set; } = string.Empty;
        public string CardOwner { get; set; } = string.Empty;

        // API وب‌سایت کاتالوگ محصولات
        public string WebApiUrl { get; set; } = string.Empty;
        public string WebApiKey { get; set; } = string.Empty;
    }
}
