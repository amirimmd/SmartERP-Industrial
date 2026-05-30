using Microsoft.EntityFrameworkCore;
using SmartERP.Core;

namespace SmartERP.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OfficialLetter> OfficialLetters { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<CustomerActivity> CustomerActivities { get; set; }
        public DbSet<CustomerReminder> CustomerReminders { get; set; }
        public DbSet<AfterSalesTicket> AfterSalesTickets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanySetting>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Invoice>().HasKey(i => i.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<OfficialLetter>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Personnel>().HasKey(p => p.Id);
            modelBuilder.Entity<CustomerActivity>().HasKey(a => a.Id);
            modelBuilder.Entity<CustomerReminder>().HasKey(r => r.Id);
            modelBuilder.Entity<AfterSalesTicket>().HasKey(t => t.Id);

            modelBuilder.Entity<CompanySetting>().HasData(
                new CompanySetting
                {
                    Id = 1,
                    CompanyName = "گروه صنعتی مشعوف",
                    LegalName = "شرکت گروه صنعتی مشعوف",
                    ManagerName = "حسام مشعوف",
                    Address = "جاده قادیکلا بزرگ، ۵۰۰ متر بعد از پمپ بنزین، جنب تالار مسعود",
                    PhoneNumber = "09110000000",
                    Slogan = "تولید کننده انواع چهارچوب فلزی و درب ضد سرقت",
                    FooterNote = "با تشکر از اعتماد شما — گروه صنعتی مشعوف",
                    LogoPath = "",
                    DefaultTaxRate = 0.09m,
                    DefaultTheme = "dark",
                    WebApiUrl = "",
                    WebApiKey = ""
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1, Code = "DOOR-TRK",
                    Name = "درب ضد سرقت روکش راش (طرح ترک)",
                    Category = "محصول نهایی", SubCategory = "درب",
                    Price = 8500000, PurchasePrice = 6000000, MinSalePrice = 7000000,
                    StockQuantity = 50, Unit = "عدد", MinStockAlert = 5,
                    Material = "فلز و MDF", IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 2, Code = "FRM-2MM",
                    Name = "چهارچوب فلزی فرانسوی (ورق ۲ میل)",
                    Category = "محصول نهایی", SubCategory = "چهارچوب",
                    Price = 1200000, PurchasePrice = 800000, MinSalePrice = 1000000,
                    StockQuantity = 200, Unit = "عدد", MinStockAlert = 20,
                    Material = "ورق فلزی", IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 3, Code = "PROF-60",
                    Name = "پروفیل آلومینیومی ۶۰ میلیمتر",
                    Category = "مواد اولیه", SubCategory = "پروفیل",
                    Price = 450000, PurchasePrice = 350000, MinSalePrice = 400000,
                    StockQuantity = 500, Unit = "متر", MinStockAlert = 50,
                    Material = "آلومینیوم", IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    Id = 4, Code = "PWD-BLK",
                    Name = "پودر رنگ مشکی مات (الکترواستاتیک)",
                    Category = "مواد اولیه", SubCategory = "پودر رنگ",
                    Price = 280000, PurchasePrice = 200000, MinSalePrice = 250000,
                    StockQuantity = 80, Unit = "کیلوگرم", MinStockAlert = 10,
                    Material = "ترموست", Color = "مشکی مات", IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<Personnel>().HasData(
                new Personnel
                {
                    Id = 1, FullName = "علی رضایی", Role = "نصاب",
                    PhoneNumber = "09121234567", Skills = "نصب درب، جوشکاری", Status = "فعال",
                    IsActive = true, HireDate = new DateTime(2023, 1, 1)
                },
                new Personnel
                {
                    Id = 2, FullName = "مهدی کریمی", Role = "تکنیسین",
                    PhoneNumber = "09359876543", Skills = "تعمیر، بازدید دوره‌ای", Status = "فعال",
                    IsActive = true, HireDate = new DateTime(2023, 6, 1)
                }
            );
        }
    }
}
