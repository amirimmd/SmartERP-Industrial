using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartERP.Infrastructure
{
    // این کلاس فقط توسط ابزار dotnet-ef در زمان توسعه استفاده می‌شود
    // و هیچ تأثیری بر اجرای برنامه ندارد.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var opt = new DbContextOptionsBuilder<AppDbContext>();
            opt.UseSqlite("Data Source=SmartERP_DesignTime.db");
            return new AppDbContext(opt.Options);
        }
    }
}
