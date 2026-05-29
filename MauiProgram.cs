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
                    // اگر از مایگریشن استفاده نمی‌کنید و دیتابیس فقط لوکال است، 
                    // ساختار دیتابیس در صورت عدم وجود ایجاد می‌شود.
                    db.Database.EnsureCreated();
                    
                    // بررسی وجود جداول (بدون FirstOrDefault برای جلوگیری از خطاهای ستون‌های جدید در SQLite)
                    // اگر ستون جدیدی اضافه کرده‌اید و از EF Core Migrations استفاده نمی‌کنید،
                    // در محیط پروداکشن SQLite اجازه تغییر Schema را به راحتی نمی‌دهد و باید دستی هندل شود 
                    // یا پایگاه داده ریست شود. اما برای جلوگیری از فاجعه حذف ناخواسته داده‌ها، متد EnsureDeleted حذف شد.
                }
                catch (Exception ex)
                {
                    // خطا در کنسول دیباگ لاگ می‌شود تا متوجه مشکل دیتابیس شوید.
                    // هرگز db.Database.EnsureDeleted() را در اینجا صدا نزنید!
                    System.Diagnostics.Debug.WriteLine($"Database Initialization Error: {ex.Message}");
                }
            }

            return app;
        }
    }
}