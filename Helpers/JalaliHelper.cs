using System.Globalization;

namespace SmartERP.Helpers
{
    public static class JalaliHelper
    {
        private static readonly PersianCalendar _pc = new();

        public static (int Year, int Month, int Day) ToJalali(DateTime dt)
            => (_pc.GetYear(dt), _pc.GetMonth(dt), _pc.GetDayOfMonth(dt));

        public static string ToJalaliString(DateTime dt)
        {
            var (y, m, d) = ToJalali(dt);
            return $"{y:D4}/{m:D2}/{d:D2}";
        }

        public static DateTime ToGregorian(int year, int month, int day)
            => _pc.ToDateTime(year, month, day, 0, 0, 0, 0);

        public static (int Year, int Month, int Day) Today()
            => ToJalali(DateTime.Today);

        public static string MonthName(int month) => month switch
        {
            1  => "فروردین",  2 => "اردیبهشت", 3 => "خرداد",
            4  => "تیر",      5 => "مرداد",     6 => "شهریور",
            7  => "مهر",      8 => "آبان",       9 => "آذر",
            10 => "دی",      11 => "بهمن",      12 => "اسفند",
            _  => ""
        };

        // نام اختصاری ستون‌های تقویم (شنبه اول هفته)
        public static readonly string[] WeekDayHeaders =
            { "شن", "یک", "دو", "سه", "چه", "پن", "جم" };

        // نام کامل روزهای هفته
        public static string DayOfWeekName(DayOfWeek dow) => dow switch
        {
            DayOfWeek.Saturday  => "شنبه",
            DayOfWeek.Sunday    => "یک‌شنبه",
            DayOfWeek.Monday    => "دوشنبه",
            DayOfWeek.Tuesday   => "سه‌شنبه",
            DayOfWeek.Wednesday => "چهارشنبه",
            DayOfWeek.Thursday  => "پنج‌شنبه",
            DayOfWeek.Friday    => "جمعه",
            _                   => ""
        };

        // تعطیلات رسمی ثابت (ماه/روز شمسی)
        private static readonly HashSet<(int M, int D)> _officialHolidays = new()
        {
            (1,1),(1,2),(1,3),(1,4),(1,12),(1,13),   // نوروز + سیزده‌به‌در
            (3,14),(3,15),                             // رحلت امام / انتقال امام
            (11,22),                                   // پیروزی انقلاب
            (12,29)                                    // ملی‌شدن صنعت نفت
        };

        public static bool IsOfficialHoliday(int month, int day)
            => _officialHolidays.Contains((month, day));

        public static bool IsFriday(DateTime gregorianDate)
            => gregorianDate.DayOfWeek == DayOfWeek.Friday;

        public static int DaysInJalaliMonth(int year, int month)
            => _pc.GetDaysInMonth(year, month);

        // فاصله (offset) اولین روز ماه از ستون شنبه (0=شنبه … 6=جمعه)
        public static int FirstDayOffset(int year, int month)
        {
            var greg = ToGregorian(year, month, 1);
            // Sat=6→0, Sun=0→1, Mon=1→2, Tue=2→3, Wed=3→4, Thu=4→5, Fri=5→6
            return ((int)greg.DayOfWeek + 1) % 7;
        }

        public static (int Year, int Month) PrevMonth(int y, int m)
            => m == 1 ? (y - 1, 12) : (y, m - 1);

        public static (int Year, int Month) NextMonth(int y, int m)
            => m == 12 ? (y + 1, 1) : (y, m + 1);
    }
}
