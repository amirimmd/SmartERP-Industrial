namespace SmartERP.Core
{
    // آیتم سفارش چهارچوب فلزی — به‌صورت JSON در Invoice.FrameOrdersJson ذخیره می‌شود
    public class FrameOrderItem
    {
        public string Name { get; set; } = string.Empty;
        // چهارچوب فرانسوی | چهارچوب مکزیکی
        public string FrameType { get; set; } = "چهارچوب فرانسوی";
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        // راست‌باز | چپ‌باز
        public string OpeningDirection { get; set; } = "راست‌باز";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal TaxRate { get; set; } = 0.09m;
    }
}
