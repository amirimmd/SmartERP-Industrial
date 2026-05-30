using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CompanySetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        // نام حقوقی / ثبتی
        public string LegalName { get; set; } = string.Empty;

        // شناسه مالیاتی / شماره ثبت
        public string TaxId { get; set; } = string.Empty;

        public string ManagerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string SecondaryPhone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Slogan { get; set; } = string.Empty;

        // متن پاورقی در فاکتورها
        public string FooterNote { get; set; } = string.Empty;

        // لوگو (Base64 یا مسیر)
        public string LogoPath { get; set; } = string.Empty;

        // امضای مدیرعامل (Base64)
        public string CeoSignatureBase64 { get; set; } = string.Empty;

        public decimal DefaultTaxRate { get; set; } = 0.09m;

        // تم پیش‌فرض: dark | light
        public string DefaultTheme { get; set; } = "dark";

        // اطلاعات بانکی
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ShabaNumber { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CardOwner { get; set; } = string.Empty;

        // API وب‌سایت کاتالوگ محصولات
        public string WebApiUrl { get; set; } = string.Empty;
        public string WebApiKey { get; set; } = string.Empty;
    }
}
