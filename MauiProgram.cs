using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SmartERP.Infrastructure;
using System.IO;
using System;
using Microsoft.Extensions.DependencyInjection;

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

            // مسیر ذخیره‌سازی دیتابیس
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartERP_Database.db");
            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            var app = builder.Build();

            // 🚀 ترفند صنعتی: ساخت دیتابیس فقط یک‌بار در زمان استارت هسته سیستم
            // این کار باعث می‌شود تمام تب‌ها بدون نیاز به چک کردن دیتابیس با حداکثر سرعت لود شوند
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated(); // ایجاد دیتابیس و جداول جدید در صورت عدم وجود
            }

            return app;
        }
    }
}