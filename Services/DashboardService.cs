using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IStudentRepository _studentRepository;

    public DashboardService(
        ApplicationDbContext context,
        IAttendanceRepository attendanceRepository,
        IPaymentRepository paymentRepository,
        IExpenseRepository expenseRepository,
        IStudentRepository studentRepository)
    {
        _context = context;
        _attendanceRepository = attendanceRepository;
        _paymentRepository = paymentRepository;
        _expenseRepository = expenseRepository;
        _studentRepository = studentRepository;
    }

    public async Task<int> GetTotalStudentsAsync() => await _studentRepository.GetActiveStudentCountAsync();

    public async Task<int> GetTotalGroupsAsync() => await _context.Groups.CountAsync(g => g.IsActive && !g.IsDeleted);

    public async Task<int> GetTodayPresentAsync() => await _attendanceRepository.GetTodayPresentCountAsync();

    public async Task<int> GetTodayAbsentAsync() => await _attendanceRepository.GetTodayAbsentCountAsync();

    public async Task<decimal> GetMonthlyRevenueAsync()
    {
        var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        return await _paymentRepository.GetTotalRevenueAsync(start, end);
    }

    public async Task<decimal> GetMonthlyExpensesAsync()
    {
        var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        return await _expenseRepository.GetTotalExpensesAsync(start, end);
    }

    public async Task<decimal> GetNetProfitAsync()
        => await GetMonthlyRevenueAsync() - await GetMonthlyExpensesAsync();

    public async Task<Dictionary<string, decimal>> GetMonthlyRevenueChartAsync()
    {
        var result = new Dictionary<string, decimal>();
        var months = new[] { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

        for (int i = 0; i < 6; i++)
        {
            var date = DateTime.Now.AddMonths(-5 + i);
            var start = new DateTime(date.Year, date.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var revenue = await _paymentRepository.GetTotalRevenueAsync(start, end);
            result[months[date.Month - 1]] = revenue;
        }
        return result;
    }

    public async Task<Dictionary<string, int>> GetMonthlyAttendanceChartAsync()
    {
        var result = new Dictionary<string, int>();
        for (int i = 5; i >= 0; i--)
        {
            var date = DateTime.Now.AddMonths(-i);
            var count = await _context.Attendances
                .CountAsync(a => a.Date.Month == date.Month && a.Date.Year == date.Year && a.Status == AttendanceStatus.Present);
            result[$"{date.Month}/{date.Year}"] = count;
        }
        return result;
    }

    public async Task<Dictionary<string, int>> GetStudentGrowthChartAsync()
    {
        var result = new Dictionary<string, int>();
        for (int i = 5; i >= 0; i--)
        {
            var date = DateTime.Now.AddMonths(-i);
            var count = await _context.Students
                .CountAsync(s => s.JoinDate.Month == date.Month && s.JoinDate.Year == date.Year && !s.IsDeleted);
            result[$"{date.Month}/{date.Year}"] = count;
        }
        return result;
    }

    public async Task<int> GetActiveGroupsCountAsync()
        => await _context.Groups.CountAsync(g => g.IsActive && !g.IsDeleted);
}
