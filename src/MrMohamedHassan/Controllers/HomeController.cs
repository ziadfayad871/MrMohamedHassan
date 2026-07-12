using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalStudents = await _dashboardService.GetTotalStudentsAsync(),
            TotalGroups = await _dashboardService.GetTotalGroupsAsync(),
            TodayPresent = await _dashboardService.GetTodayPresentAsync(),
            TodayAbsent = await _dashboardService.GetTodayAbsentAsync(),
            MonthlyRevenue = await _dashboardService.GetMonthlyRevenueAsync(),
            MonthlyExpenses = await _dashboardService.GetMonthlyExpensesAsync(),
            NetProfit = await _dashboardService.GetNetProfitAsync(),
            ActiveGroups = await _dashboardService.GetActiveGroupsCountAsync(),
            MonthlyRevenueChart = await _dashboardService.GetMonthlyRevenueChartAsync(),
            MonthlyAttendanceChart = await _dashboardService.GetMonthlyAttendanceChartAsync(),
            StudentGrowthChart = await _dashboardService.GetStudentGrowthChartAsync()
        };
        return View(model);
    }

    public IActionResult Error(int? statusCode = null)
    {
        return View();
    }
}
