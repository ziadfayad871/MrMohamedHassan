namespace MrMohamedHassan.Helpers;

public static class NumberHelper
{
    public static string ToCurrency(this decimal amount, string currency = "ج.م")
    {
        return $"{amount:N2} {currency}";
    }

    public static string ToPercentage(this double value)
    {
        return $"{value:F1}%";
    }
}
