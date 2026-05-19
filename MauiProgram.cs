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
            // اصلاح خطا: استفاده از نام صحیح متد CreateBuilder به جای CreateMauiAppBuilder
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

            // استفاده از دستور بومی سی‌شارپ برای جلوگیری از کرش کردن ویندوز قبل از لود برنامه
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartERP_Database.db");
            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            return builder.Build();
        }
    }
}