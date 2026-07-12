namespace MrMohamedHassan.Helpers;

public static class DateTimeHelper
{
    public static string ToArabicDate(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy");
    }

    public static string ToArabicDateTime(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy hh:mm tt");
    }

    public static string GetDayName(this DateTime date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Saturday => "السبت",
            DayOfWeek.Sunday => "الأحد",
            DayOfWeek.Monday => "الإثنين",
            DayOfWeek.Tuesday => "الثلاثاء",
            DayOfWeek.Wednesday => "الأربعاء",
            DayOfWeek.Thursday => "الخميس",
            DayOfWeek.Friday => "الجمعة",
            _ => ""
        };
    }

    public static string GetMonthName(this int month)
    {
        return month switch
        {
            1 => "يناير", 2 => "فبراير", 3 => "مارس", 4 => "أبريل",
            5 => "مايو", 6 => "يونيو", 7 => "يوليو", 8 => "أغسطس",
            9 => "سبتمبر", 10 => "أكتوبر", 11 => "نوفمبر", 12 => "ديسمبر",
            _ => ""
        };
    }
}
