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

            // IDbContextFactory pattern - الگوی صحیح برای MAUI Blazor Hybrid
            // هر عملیات DB یک instance مجزا می‌گیرد → هیچ‌وقت concurrent access error نمی‌دهد
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // AddTransient برای backward compatibility با صفحاتی که AppDbContext inject می‌کنند
            builder.Services.AddTransient<AppDbContext>(sp =>
                sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            var app = builder.Build();

            // ایجاد و اعتبارسنجی schema دیتابیس هنگام استارت
            var dbFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
            using (var db = dbFactory.CreateDbContext())
            {
                try
                {
                    db.Database.EnsureCreated();
                    // FirstOrDefault() mapping کامل ستون‌ها را چک می‌کند
                    // اگر ستون جدیدی اضافه شده باشد و در DB نباشد، exception می‌دهد
                    _ = db.CompanySettings.FirstOrDefault();
                    _ = db.Products.FirstOrDefault();
                    _ = db.Invoices.FirstOrDefault();
                    _ = db.Customers.FirstOrDefault();
                    _ = db.Orders.FirstOrDefault();
                    _ = db.OfficialLetters.FirstOrDefault();
                }
                catch (Exception)
                {
                    // Schema قدیمی → بازسازی کامل
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            }

            return app;
        }
    }
}
