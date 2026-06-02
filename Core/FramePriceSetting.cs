using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class FramePriceSetting
    {
        [Key]
        public int Id { get; set; }

        // چهارچوب فرانسوی | چهارچوب مکزیکی
        public string FrameType { get; set; } = "چهارچوب فرانسوی";

        // چپ‌باز | راست‌باز
        public string OpeningDirection { get; set; } = "راست‌باز";

        public decimal BasePrice { get; set; } = 0;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
