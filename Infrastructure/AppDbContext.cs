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

            modelBuilder.Entity<CompanySetting>().HasData(
                new CompanySetting
                {
                    Id = 1,
                    CompanyName = "گروه صنعتی مشعوف",
                    ManagerName = "حسام مشعوف",
                    Address = "جاده قادیکلا بزرگ، 500 متر بعد از پمپ بنزین، جنب تالار مسعود",
                    PhoneNumber = "09110000000",
                    Slogan = "تولید کننده انواع چهارچوب فلزی و درب ضد سرقت",
                    LogoPath = "/images/default-logo.png",
                    DefaultTaxRate = 0.09m,
                    WebApiUrl = "",
                    WebApiKey = ""
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Code = "DOOR-TRK", Name = "درب ضد سرقت روکش راش (طرح ترک)", Price = 8500000, StockQuantity = 50, Unit = "عدد" },
                new Product { Id = 2, Code = "FRM-2MM", Name = "چهارچوب فلزی فرانسوی (ورق 2 میل)", Price = 1200000, StockQuantity = 200, Unit = "عدد" }
            );
        }
    }
}
