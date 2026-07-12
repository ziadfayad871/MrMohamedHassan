using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Repositories;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;
using System.Security.Claims;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class PaymentsController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IStudentService _studentService;
    private readonly ISettingRepository _settingRepository;
    private readonly IExportService _exportService;
    private readonly ApplicationDbContext _context;

    public PaymentsController(
        IPaymentService paymentService,
        IStudentService studentService,
        ISettingRepository settingRepository,
        IExportService exportService,
        ApplicationDbContext context)
    {
        _paymentService = paymentService;
        _studentService = studentService;
        _settingRepository = settingRepository;
        _exportService = exportService;
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var payments = await _paymentService.GetAllAsync();
        var pageSize = 15;
        var paged = payments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModel = paged.Select(p => new PaymentListViewModel
        {
            Id = p.Id,
            StudentId = p.StudentId,
            StudentName = p.Student?.FullName ?? "",
            StudentCode = p.Student?.StudentCode ?? "",
            Amount = p.Amount,
            Discount = p.Discount,
            PaidAmount = p.PaidAmount,
            PaymentType = p.PaymentType,
            PaymentMethod = p.PaymentMethod,
            PaymentDate = p.PaymentDate,
            ReceiptNumber = p.ReceiptNumber,
            Notes = p.Notes
        }).ToList();

        ViewBag.TotalCount = payments.Count();
        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling(payments.Count() / (double)pageSize);

        return View(viewModel);
    }

    public async Task<IActionResult> Create(int? studentId)
    {
        ViewBag.Students = await _studentService.GetAllAsync();
        var model = new PaymentCreateViewModel { StudentId = studentId ?? 0 };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Students = await _studentService.GetAllAsync();
            return View(model);
        }

        var payment = new MrMohamedHassan.Models.Payment
        {
            StudentId = model.StudentId,
            Amount = model.Amount,
            Discount = model.Discount,
            PaymentType = model.PaymentType,
            PaymentMethod = model.PaymentMethod,
            PaymentDate = model.PaymentDate,
            Notes = model.Notes,
            CreatedById = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
        };

        var created = await _paymentService.CreateAsync(payment);
        TempData["Success"] = "تم تسجيل الدفعة بنجاح";
        return RedirectToAction(nameof(Receipt), new { id = created.Id });
    }

    public async Task<IActionResult> Receipt(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null) return NotFound();

        var model = new PaymentReceiptViewModel
        {
            PaymentId = payment.Id,
            ReceiptNumber = payment.ReceiptNumber ?? "",
            StudentName = payment.Student?.FullName ?? "",
            StudentCode = payment.Student?.StudentCode ?? "",
            Amount = payment.Amount,
            Discount = payment.Discount,
            PaidAmount = payment.PaidAmount,
            PaymentDate = payment.PaymentDate,
            PaymentType = payment.PaymentType.ToString(),
            PaymentMethod = payment.PaymentMethod.ToString(),
            CenterName = await _settingRepository.GetValueAsync("CenterName") ?? "مركز محمد حسن",
            CenterPhone = await _settingRepository.GetValueAsync("CenterPhone") ?? ""
        };

        return View(model);
    }

    public async Task<IActionResult> ExportExcel(DateTime? start, DateTime? end)
    {
        var startDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = end ?? DateTime.Now;
        var payments = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);

        var dict = new Dictionary<string, string>
        {
            { "ReceiptNumber", "رقم الإيصال" },
            { "StudentCode", "كود الطالب" },
            { "Amount", "المبلغ" },
            { "Discount", "الخصم" },
            { "PaidAmount", "المدفوع" },
            { "PaymentType", "النوع" },
            { "PaymentMethod", "طريقة الدفع" },
            { "PaymentDate", "التاريخ" }
        };

        var data = payments.Select(p => new
        {
            p.ReceiptNumber,
            StudentCode = p.Student?.StudentCode ?? "",
            p.Amount,
            p.Discount,
            p.PaidAmount,
            PaymentType = p.PaymentType.ToString(),
            PaymentMethod = p.PaymentMethod.ToString(),
            PaymentDate = p.PaymentDate.ToString("yyyy-MM-dd")
        });

        var excel = _exportService.ExportToExcel(data, "المدفوعات", dict);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "payments.xlsx");
    }
}
