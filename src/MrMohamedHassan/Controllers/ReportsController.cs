using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Services;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;
    private readonly IExportService _exportService;
    private readonly IGroupService _groupService;

    public ReportsController(IReportService reportService, IExportService exportService, IGroupService groupService)
    {
        _reportService = reportService;
        _exportService = exportService;
        _groupService = groupService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> StudentReport(int studentId)
    {
        var report = await _reportService.GenerateStudentReportAsync(studentId);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"student_{studentId}_report.xlsx");
    }

    public async Task<IActionResult> AttendanceReport(int groupId, DateTime? start, DateTime? end)
    {
        var startDate = start ?? DateTime.Now.AddMonths(-1);
        var endDate = end ?? DateTime.Now;
        var report = await _reportService.GenerateAttendanceReportAsync(groupId, startDate, endDate);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "attendance_report.xlsx");
    }

    public async Task<IActionResult> PaymentReport(DateTime? start, DateTime? end)
    {
        var startDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = end ?? DateTime.Now;
        var report = await _reportService.GeneratePaymentReportAsync(startDate, endDate);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "payment_report.xlsx");
    }

    public async Task<IActionResult> FinancialReport(DateTime? start, DateTime? end)
    {
        var startDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = end ?? DateTime.Now;
        var report = await _reportService.GenerateFinancialReportAsync(startDate, endDate);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "financial_report.xlsx");
    }

    public async Task<IActionResult> ExamReport(int examId)
    {
        var report = await _reportService.GenerateExamReportAsync(examId);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exam_{examId}_report.xlsx");
    }
}
