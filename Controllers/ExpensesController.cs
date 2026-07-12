using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;
using System.Security.Claims;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class ExpensesController : Controller
{
    private readonly IExpenseService _expenseService;
    private readonly IExportService _exportService;

    public ExpensesController(IExpenseService expenseService, IExportService exportService)
    {
        _expenseService = expenseService;
        _exportService = exportService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var expenses = await _expenseService.GetAllAsync();
        var pageSize = 15;
        var paged = expenses.OrderByDescending(e => e.ExpenseDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModel = paged.Select(e => new ExpenseListViewModel
        {
            Id = e.Id,
            Description = e.Description,
            Amount = e.Amount,
            ExpenseCategoryId = e.ExpenseCategoryId,
            CategoryName = e.Category?.Name ?? "",
            ExpenseDate = e.ExpenseDate,
            Notes = e.Notes
        }).ToList();

        ViewBag.TotalCount = expenses.Count();
        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling(expenses.Count() / (double)pageSize);

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _expenseService.GetCategoriesAsync();
        return View(new ExpenseCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpenseCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _expenseService.GetCategoriesAsync();
            return View(model);
        }

        var expense = new Expense
        {
            Description = model.Description,
            Amount = model.Amount,
            ExpenseCategoryId = model.ExpenseCategoryId,
            ExpenseDate = model.ExpenseDate,
            Notes = model.Notes,
            CreatedById = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
        };

        await _expenseService.CreateAsync(expense);
        TempData["Success"] = "تم تسجيل المصروف بنجاح";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _expenseService.DeleteAsync(id);
        TempData["Success"] = "تم حذف المصروف بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportExcel(DateTime? start, DateTime? end)
    {
        var startDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = end ?? DateTime.Now;
        var expenses = await _expenseService.GetExpensesByDateRangeAsync(startDate, endDate);

        var dict = new Dictionary<string, string>
        {
            { "Description", "الوصف" },
            { "CategoryName", "الفئة" },
            { "Amount", "المبلغ" },
            { "ExpenseDate", "التاريخ" },
            { "Notes", "ملاحظات" }
        };

        var data = expenses.Select(e => new
        {
            e.Description,
            CategoryName = e.Category?.Name ?? "",
            e.Amount,
            ExpenseDate = e.ExpenseDate.ToString("yyyy-MM-dd"),
            e.Notes
        });

        var excel = _exportService.ExportToExcel(data, "المصروفات", dict);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "expenses.xlsx");
    }
}
