namespace MrMohamedHassan.ViewModels;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }
    public int TotalGroups { get; set; }
    public int TodayPresent { get; set; }
    public int TodayAbsent { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public int ActiveGroups { get; set; }
    public Dictionary<string, decimal> MonthlyRevenueChart { get; set; } = new();
    public Dictionary<string, int> MonthlyAttendanceChart { get; set; } = new();
    public Dictionary<string, int> StudentGrowthChart { get; set; } = new();
    public List<LatestStudentViewModel> LatestStudents { get; set; } = new();
    public List<LatestPaymentViewModel> LatestPayments { get; set; } = new();
}

public class LatestStudentViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public string? ImageUrl { get; set; }
}

public class LatestPaymentViewModel
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReceiptNumber { get; set; }
}
