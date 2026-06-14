using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartERP.Infrastructure;
using System;
using System.IO;
using System.Windows;

namespace SmartERP
{
    public partial class App : Application
    {
        /// <summary>
        /// The single application-wide DI container, set once during startup.
        /// MainWindow reads this to inject services into BlazorWebView.
        /// </summary>
        public static IServiceProvider Services { get; private set; } = null!;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Register WPF BlazorWebView support (equivalent to AddMauiBlazorWebView)
            services.AddWpfBlazorWebView();

#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
            services.AddLogging(lb => lb.AddDebug());
#endif

            // SQLite database stored in the same path as the MAUI version,
            // so an existing database can be reused without data migration.
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SmartERP_Database.db");

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Transient registration so individual Razor components can inject
            // AppDbContext directly (compatible with the existing component code).
            services.AddTransient<AppDbContext>(sp =>
                sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            Services = services.BuildServiceProvider();

            // Initialize / upgrade the database schema before showing any UI
            var dbFactory = Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
            using (var db = dbFactory.CreateDbContext())
            {
                try
                {
                    db.Database.EnsureCreated();
                    ApplySchemaUpgrades(db);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database Init Error: {ex.Message}");
                }
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        /// <summary>
        /// Applies incremental DDL changes to an existing SQLite database.
        /// Identical to the method in MauiProgram.cs — any future changes must
        /// be kept in sync in both projects until the MAUI build is retired.
        /// </summary>
        private static void ApplySchemaUpgrades(AppDbContext db)
        {
            // ─── Create new tables (safe — uses IF NOT EXISTS) ────────────────
            var newTables = new[]
            {
                @"CREATE TABLE IF NOT EXISTS ""Personnel"" (
                    ""Id""           INTEGER NOT NULL CONSTRAINT ""PK_Personnel"" PRIMARY KEY AUTOINCREMENT,
                    ""FullName""     TEXT    NOT NULL DEFAULT '',
                    ""Role""         TEXT    NOT NULL DEFAULT 'نصاب',
                    ""PhoneNumber""  TEXT    NOT NULL DEFAULT '',
                    ""NationalId""   TEXT    NOT NULL DEFAULT '',
                    ""Skills""       TEXT    NOT NULL DEFAULT '',
                    ""AvatarBase64"" TEXT    NOT NULL DEFAULT '',
                    ""Status""       TEXT    NOT NULL DEFAULT 'فعال',
                    ""Notes""        TEXT    NOT NULL DEFAULT '',
                    ""HireDate""     TEXT    NOT NULL DEFAULT '2024-01-01 00:00:00',
                    ""IsActive""     INTEGER NOT NULL DEFAULT 1
                )",

                @"CREATE TABLE IF NOT EXISTS ""CustomerReminders"" (
                    ""Id""           INTEGER NOT NULL CONSTRAINT ""PK_CustomerReminders"" PRIMARY KEY AUTOINCREMENT,
                    ""CustomerId""   INTEGER,
                    ""CustomerName"" TEXT    NOT NULL DEFAULT '',
                    ""ReminderDate"" TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""Title""        TEXT    NOT NULL DEFAULT '',
                    ""Description""  TEXT    NOT NULL DEFAULT '',
                    ""IsDismissed""  INTEGER NOT NULL DEFAULT 0,
                    ""Priority""     TEXT    NOT NULL DEFAULT 'متوسط',
                    ""CreatedAt""    TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP
                )",

                @"CREATE TABLE IF NOT EXISTS ""CustomerActivities"" (
                    ""Id""           INTEGER NOT NULL CONSTRAINT ""PK_CustomerActivities"" PRIMARY KEY AUTOINCREMENT,
                    ""CustomerId""   INTEGER NOT NULL DEFAULT 0,
                    ""ActivityDate"" TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""ActivityType"" TEXT    NOT NULL DEFAULT 'تماس',
                    ""Subject""      TEXT    NOT NULL DEFAULT '',
                    ""Description""  TEXT    NOT NULL DEFAULT '',
                    ""Outcome""      TEXT    NOT NULL DEFAULT '',
                    ""OperatorName"" TEXT    NOT NULL DEFAULT 'مدیر سیستم'
                )",

                @"CREATE TABLE IF NOT EXISTS ""AfterSalesTickets"" (
                    ""Id""                      INTEGER NOT NULL CONSTRAINT ""PK_AfterSalesTickets"" PRIMARY KEY AUTOINCREMENT,
                    ""InvoiceId""               INTEGER,
                    ""InvoiceNumber""            TEXT    NOT NULL DEFAULT '',
                    ""CustomerName""             TEXT    NOT NULL DEFAULT '',
                    ""CustomerPhone""            TEXT    NOT NULL DEFAULT '',
                    ""IssueType""               TEXT    NOT NULL DEFAULT 'خرابی',
                    ""Description""              TEXT    NOT NULL DEFAULT '',
                    ""Status""                  TEXT    NOT NULL DEFAULT 'باز',
                    ""AssignedTechnicianId""     INTEGER,
                    ""AssignedTechnicianName""   TEXT    NOT NULL DEFAULT '',
                    ""ReportedDate""             TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""ResolvedDate""             TEXT,
                    ""Resolution""              TEXT    NOT NULL DEFAULT ''
                )",

                @"CREATE TABLE IF NOT EXISTS ""CalendarEvents"" (
                    ""Id""           INTEGER NOT NULL CONSTRAINT ""PK_CalendarEvents"" PRIMARY KEY AUTOINCREMENT,
                    ""JalaliDate""   TEXT    NOT NULL DEFAULT '',
                    ""EventDate""    TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""Title""        TEXT    NOT NULL DEFAULT '',
                    ""EventType""    TEXT    NOT NULL DEFAULT 'یادآوری',
                    ""Description""  TEXT    NOT NULL DEFAULT '',
                    ""AuthorRole""   TEXT    NOT NULL DEFAULT 'عمومی',
                    ""AuthorName""   TEXT    NOT NULL DEFAULT 'مدیر سیستم',
                    ""CreatedAt""    TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""InvoiceId""    INTEGER,
                    ""InvoiceNumber"" TEXT   NOT NULL DEFAULT '',
                    ""OrderId""      INTEGER,
                    ""OrderNumber""  TEXT    NOT NULL DEFAULT '',
                    ""CheckAmount""  REAL,
                    ""CheckNumber""  TEXT    NOT NULL DEFAULT ''
                )",

                @"CREATE TABLE IF NOT EXISTS ""FramePriceSettings"" (
                    ""Id""               INTEGER NOT NULL CONSTRAINT ""PK_FramePriceSettings"" PRIMARY KEY AUTOINCREMENT,
                    ""FrameType""        TEXT    NOT NULL DEFAULT 'چهارچوب فرانسوی',
                    ""OpeningDirection"" TEXT    NOT NULL DEFAULT 'راست‌باز',
                    ""BasePrice""        REAL    NOT NULL DEFAULT 0,
                    ""UpdatedAt""        TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP
                )"
            };

            foreach (var sql in newTables)
            {
                try { db.Database.ExecuteSqlRaw(sql); }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Table create error: {ex.Message}");
                }
            }

            // ─── Add new columns to existing tables (ALTER TABLE) ─────────────
            // Each statement is silently ignored if the column already exists.
            var columnUpgrades = new[]
            {
                // ── Products ──────────────────────────────────────────────────
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

                // ── Customers ─────────────────────────────────────────────────
                "ALTER TABLE \"Customers\" ADD COLUMN \"City\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"Province\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"LeadSource\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Customers\" ADD COLUMN \"ReferredBy\" TEXT NOT NULL DEFAULT ''",

                // ── Invoices ──────────────────────────────────────────────────
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

                // ── CompanySettings ───────────────────────────────────────────
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"LegalName\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"TaxId\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"SecondaryPhone\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"Website\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"Email\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"FooterNote\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"CeoSignatureBase64\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"DefaultTheme\" TEXT NOT NULL DEFAULT 'dark'",
                "ALTER TABLE \"CompanySettings\" ADD COLUMN \"CardNumber\" TEXT NOT NULL DEFAULT ''",

                // ── Orders (v2: production workflow) ──────────────────────────
                "ALTER TABLE \"Orders\" ADD COLUMN \"ProductionPriority\" TEXT NOT NULL DEFAULT 'عادی'",
                "ALTER TABLE \"Orders\" ADD COLUMN \"ProductionStartDate\" TEXT",
                "ALTER TABLE \"Orders\" ADD COLUMN \"CompletedAt\" TEXT",

                // ── Invoices (v2: frame + structured plate + custom tax) ───────
                "ALTER TABLE \"Invoices\" ADD COLUMN \"FrameOrderEnabled\" INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"FrameOrdersJson\" TEXT NOT NULL DEFAULT '[]'",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"VehiclePlatePart1\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"VehiclePlateLetter\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"VehiclePlatePart2\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"VehiclePlateRegion\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"Invoices\" ADD COLUMN \"CustomTaxRate\" REAL NOT NULL DEFAULT -1",

                // ── Products (v2: metal frame support) ────────────────────────
                "ALTER TABLE \"Products\" ADD COLUMN \"IsFrameProduct\" INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE \"Products\" ADD COLUMN \"FrameType\" TEXT NOT NULL DEFAULT 'چهارچوب فرانسوی'",
                "ALTER TABLE \"Products\" ADD COLUMN \"OpeningDirection\" TEXT NOT NULL DEFAULT 'راست‌باز'",
                "ALTER TABLE \"Products\" ADD COLUMN \"TaxRate\" REAL NOT NULL DEFAULT -1",

                // ── Orders (v3: timed production stages) ──────────────────────
                "ALTER TABLE \"Orders\" ADD COLUMN \"ProductionStartedAt\" TEXT",
                "ALTER TABLE \"Orders\" ADD COLUMN \"ReadyAt\" TEXT",

                // ── CalendarEvents (v2: cross-references + Gregorian date) ─────
                "ALTER TABLE \"CalendarEvents\" ADD COLUMN \"EventDate\" TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP",
                "ALTER TABLE \"CalendarEvents\" ADD COLUMN \"InvoiceId\" INTEGER",
                "ALTER TABLE \"CalendarEvents\" ADD COLUMN \"InvoiceNumber\" TEXT NOT NULL DEFAULT ''",
                "ALTER TABLE \"CalendarEvents\" ADD COLUMN \"OrderId\" INTEGER",
                "ALTER TABLE \"CalendarEvents\" ADD COLUMN \"OrderNumber\" TEXT NOT NULL DEFAULT ''",
            };

            foreach (var sql in columnUpgrades)
            {
                try { db.Database.ExecuteSqlRaw(sql); }
                catch { /* column already exists — silently skip */ }
            }
        }
    }
}
