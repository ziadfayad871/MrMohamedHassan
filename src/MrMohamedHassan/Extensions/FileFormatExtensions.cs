namespace MrMohamedHassan.Extensions;

public static class StringExtensions
{
    public static string ToArabicEnumName(this string enumValue)
    {
        return enumValue switch
        {
            "Male" => "ذكر",
            "Female" => "أنثى",
            "Active" => "نشط",
            "Inactive" => "غير نشط",
            "Withdrawn" => "منسحب",
            "Graduated" => "متخرج",
            "Present" => "حاضر",
            "Absent" => "غائب",
            "Late" => "متأخر",
            "Cash" => "نقدي",
            "BankTransfer" => "تحويل بنكي",
            "VodafoneCash" => "فودافون كاش",
            "Online" => "الدفع الإلكتروني",
            "Subscription" => "اشتراك",
            "Partial" => "دفعة جزئية",
            "Extra" => "رسوم إضافية",
            "General" => "عام",
            "Payment" => "دفعة",
            "Attendance" => "حضور",
            "Exam" => "امتحان",
            "Homework" => "واجب",
            "Alert" => "تنبيه",
            _ => enumValue
        };
    }
}
