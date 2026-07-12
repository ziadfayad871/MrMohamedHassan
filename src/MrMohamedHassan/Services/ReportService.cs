using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;
    private readonly IExportService _exportService;

    public ReportService(ApplicationDbContext context, IExportService exportService)
    {
        _context = context;
        _exportService = exportService;
    }

    public async Task<byte[]> GenerateStudentReportAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null) return Array.Empty<byte>();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("تقرير الطالب");

        ws.Cell(1, 1).Value = "تقرير الطالب";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 16;

        ws.Cell(3, 1).Value = "كود الطالب:";
        ws.Cell(3, 2).Value = student.StudentCode;
        ws.Cell(4, 1).Value = "الاسم:";
        ws.Cell(4, 2).Value = student.FullName;
        ws.Cell(5, 1).Value = "الحالة:";
        ws.Cell(5, 2).Value = student.Status.ToString();

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GenerateAttendanceReportAsync(int groupId, DateTime start, DateTime end)
    {
        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .Where(a => a.GroupId == groupId && a.Date >= start && a.Date <= end)
            .ToListAsync();

        var dict = new Dictionary<string, string>
        {
            { "FullName", "اسم الطالب" },
            { "StudentCode", "كود الطالب" },
            { "Date", "التاريخ" },
            { "Status", "الحالة" }
        };

        var data = attendance.Select(a => new
        {
            a.Student.FullName,
            a.Student.StudentCode,
            Date = a.Date.ToString("yyyy-MM-dd"),
            Status = a.Status.ToString()
        });

        return _exportService.ExportToExcel(data, "تقرير الحضور", dict);
    }

    public async Task<byte[]> GeneratePaymentReportAsync(DateTime start, DateTime end)
    {
        var payments = await _context.Payments
            .Include(p => p.Student)
            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
            .ToListAsync();

        var dict = new Dictionary<string, string>
        {
            { "StudentFullName", "اسم الطالب" },
            { "Amount", "المبلغ" },
            { "PaidAmount", "المدفوع" },
            { "Discount", "الخصم" },
            { "PaymentDate", "التاريخ" },
            { "ReceiptNumber", "رقم الإيصال" }
        };

        var data = payments.Select(p => new
        {
            StudentFullName = p.Student?.FullName ?? "",
            p.Amount,
            p.PaidAmount,
            p.Discount,
            PaymentDate = p.PaymentDate.ToString("yyyy-MM-dd"),
            p.ReceiptNumber
        });

        return _exportService.ExportToExcel(data, "تقرير المدفوعات", dict);
    }

    public async Task<byte[]> GenerateFinancialReportAsync(DateTime start, DateTime end)
    {
        var revenue = await _context.Payments
            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end && p.IsPaid)
            .SumAsync(p => p.PaidAmount);

        var expenses = await _context.Expenses
            .Where(e => e.ExpenseDate >= start && e.ExpenseDate <= end)
            .SumAsync(e => e.Amount);

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("التقرير المالي");

        ws.Cell(1, 1).Value = "التقرير المالي";
        ws.Cell(3, 1).Value = "الإيرادات:";
        ws.Cell(3, 2).Value = revenue;
        ws.Cell(4, 1).Value = "المصروفات:";
        ws.Cell(4, 2).Value = expenses;
        ws.Cell(5, 1).Value = "صافي الربح:";
        ws.Cell(5, 2).Value = revenue - expenses;

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GenerateExamReportAsync(int examId)
    {
        var results = await _context.ExamResults
            .Include(r => r.Student)
            .Include(r => r.Exam)
            .Where(r => r.ExamId == examId)
            .OrderByDescending(r => r.MarksObtained)
            .ToListAsync();

        var exam = results.FirstOrDefault()?.Exam;

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("تقرير الامتحان");

        ws.Cell(1, 1).Value = $"تقرير {exam?.Title ?? ""}";
        ws.Cell(2, 1).Value = "الاسم";
        ws.Cell(2, 2).Value = "الدرجة";
        ws.Cell(2, 3).Value = "النسبة";

        int row = 3;
        foreach (var r in results)
        {
            ws.Cell(row, 1).Value = r.Student.FullName;
            ws.Cell(row, 2).Value = r.MarksObtained;
            ws.Cell(row, 3).Value = exam != null ? $"{(double)r.MarksObtained / exam.MaxMarks * 100:F1}%" : "";
            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
