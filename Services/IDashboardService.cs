namespace MrMohamedHassan.Services;

public interface IDashboardService
{
    Task<int> GetTotalStudentsAsync();
    Task<int> GetTotalGroupsAsync();
    Task<int> GetTodayPresentAsync();
    Task<int> GetTodayAbsentAsync();
    Task<decimal> GetMonthlyRevenueAsync();
    Task<decimal> GetMonthlyExpensesAsync();
    Task<decimal> GetNetProfitAsync();
    Task<Dictionary<string, decimal>> GetMonthlyRevenueChartAsync();
    Task<Dictionary<string, int>> GetMonthlyAttendanceChartAsync();
    Task<Dictionary<string, int>> GetStudentGrowthChartAsync();
    Task<int> GetActiveGroupsCountAsync();
}
