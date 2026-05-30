using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SmartERP.Infrastructure;
using System.IO;
using System;

namespace SmartERP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SmartERP_Database.db");

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            builder.Services.AddTransient<AppDbContext>(sp =>
                sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            var app = builder.Build();

            var dbFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
            using (var db = dbFactory.CreateDbContext())
            {
                try
                {
                    // ایجاد جداول جدید در نصب تازه
                    db.Database.EnsureCreated();

                    // اعمال ستون‌های جدید روی دیتابیس‌های موجود (مهاجرت بدون حذف داده)
                    ApplySchemaUpgrades(db);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database Init Error: {ex.Message}");
                }
            }

            return app;
        }

        private static void ApplySchemaUpgrades(AppDbContext db)
        {
            // هر ALTER TABLE در صورت وجود ستون با try/catch نادیده گرفته می‌شود.
            // این روش ایمن‌ترین راه برای به‌روزرسانی SQLite بدون از دست دادن داده است.

            var upgrades = new[]
            {
                // ── Products ──────────────────────────────────────────────
                "ALTER TABLE \"Products\" ADD COLUMN \"Description\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Products\" ADD COLUMN \"Category\" TEXT NOT NULL DEFAULT 'محصول نهایی'",
                "ALTER TABLE \"Products\" ADD COLUMN \"SubCategory\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Products\" ADD COLUMN \"Width\" REAL NOT NULL DEFAULT 0",
                "ALTER TABLE \"Products\" ADD COLUMN \"Height\" REAL NOT NULL DEFAULT 0",
                "ALTER TABLE \"Products\" ADD COLUMN \"Depth\" REAL NOT NULL DEFAULT 0",
                "ALTER TABLE \"Products\" ADD COLUMN \"Weight\" REAL NOT NULL DEFAULT 0",
                "ALTER TABLE \"Products\" ADD COLUMN \"Material\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Products\" ADD COLUMN \"Color\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Products\" ADD COLUMN \"VariantsJson\" TEXT NOT NULL DEFAULT '[]'",
                "ALTER TABLE \"Products\" ADD COLUMN \"ImagesJson\" TEXT NOT NULL DEFAULT '[]'",
                "ALTER TABLE \"Products\" ADD COLUMN \"PurchasePrice\" TEXT NOT NULL DEFAULT '0'",
                "ALTER TABLE \"Products\" ADD COLUMN \"MinSalePrice\" TEXT NOT NULL DEFAULT '0'",
                "ALTER TABLE \"Products\" ADD COLUMN \"MinStockAlert\" REAL NOT NULL DEFAULT 5",
                "ALTER TABLE \"Products\" ADD COLUMN \"IsActive\" INTEGER NOT NULL DEFAULT 1",
                "ALTER TABLE \"Products\" ADD COLUMN \"CreatedAt\" TEXT NOT NULL DEFAULT '2024-01-01 00:00:00'",

                // ── Customers ─────────────────────────────────────────────
                "ALTER TABLE \"Customers\" ADD COLUMN \"City\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"Province\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"LeadSource\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"ReferredBy\" TEXT NOT NULL DEFAULT ''",

                // ── Invoices ──────────────────────────────────────────────
                "ALTER TABLE \"Invoices\" ADD COLUMN \"CurrentStep\" INTEGER NOT NULL DEFAULT 1",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step1Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step2Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step3Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step4Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step5Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"Step6Notes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"ReceivedInGoodCondition\" INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"ReceiverName\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"ProductionDate\" TEXT",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"ProductionDeadline\" TEXT",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"IsUrgentProduction\" INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"InstallerPersonnelId\" INTEGER",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"InstallerName\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"InstallationDateTime\" TEXT",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"InstallationNotes\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"AfterSaleNotes\" TEXT NOT NULL DEFAULT ''",

                // ── CompanySettings ───────────────────────────────────────
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"LegalName\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"TaxId\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"SecondaryPhone\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"Website\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"Email\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"FooterNote\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"CeoSignatureBase64\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"DefaultTheme\" TEXT NOT NULL DEFAULT 'dark'",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"CardNumber\" TEXT NOT NULL DEFAULT ''",
            };

            foreach (var sql in upgrades)
            {
                try
                {
                    db.Database.ExecuteSqlRaw(sql);
                }
                catch
                {
                    // ستون از قبل وجود دارد — نادیده گرفته می‌شود
                }
            }
        }
    }
}
