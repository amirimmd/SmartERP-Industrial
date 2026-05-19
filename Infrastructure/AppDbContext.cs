using Microsoft.EntityFrameworkCore;
using SmartERP.Core;

namespace SmartERP.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        
        // نکته: در معماری جدید، جدول InvoiceItem حذف شده و اقلام فاکتور 
        // به صورت رشته‌های بهینه‌شده (JSON) درون خود کلاس Invoice ذخیره می‌شوند 
        // تا سرعت لود فاکتورها در لپ‌تاپ‌های مختلف افزایش یابد.

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // تنظیم ساختار پایه‌ای دیتابیس
            modelBuilder.Entity<CompanySetting>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Invoice>().HasKey(i => i.Id);

            // بازگرداندن اطلاعات واقعی شرکت شما (گروه صنعتی مشعوف) منطبق با مدل جدید
            modelBuilder.Entity<CompanySetting>().HasData(
                new CompanySetting
                {
                    Id = 1,
                    CompanyName = "گروه صنعتی مشعوف",
                    ManagerName = "حسام مشعوف", 
                    // تلفیق شعار و تلفن با آدرس برای نمایش یکپارچه در فاکتورها
                    Address = "جاده قادیکلا بزرگ، 500 متر بعد از پمپ بنزین، جنب تالار مسعود | تلفن: 09110000000 | شعار: تولید کننده انواع چهارچوب فلزی و درب ضد سرقت",
                    LogoPath = "/images/default-logo.png"
                }
            );

            // بازگرداندن محصولات واقعی شما منطبق با معماری جدید (کد، قیمت، واحد)
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Code = "DOOR-TRK", Name = "درب ضد سرقت روکش راش (طرح ترک)", Price = 8500000, StockQuantity = 50, Unit = "عدد" },
                new Product { Id = 2, Code = "FRM-2MM", Name = "چهارچوب فلزی فرانسوی (ورق 2 میل)", Price = 1200000, StockQuantity = 200, Unit = "عدد" }
            );
        }
    }
}