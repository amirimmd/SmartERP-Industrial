using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class Personnel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        // نصاب، تکنیسین، راننده، مدیر تولید، اداری، نگهبان
        public string Role { get; set; } = "نصاب";

        public string PhoneNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;

        // مهارت‌ها (comma-separated)
        public string Skills { get; set; } = string.Empty;

        // آواتار به صورت Base64
        public string AvatarBase64 { get; set; } = string.Empty;

        // فعال، غیرفعال، در مرخصی
        public string Status { get; set; } = "فعال";

        public string Notes { get; set; } = string.Empty;
        public DateTime HireDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
