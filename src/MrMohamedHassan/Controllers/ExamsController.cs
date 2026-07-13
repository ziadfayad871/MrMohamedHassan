using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class ExamsController : Controller
{
    private readonly IExamService _examService;
    private readonly IGroupService _groupService;
    private readonly IExportService _exportService;
    private readonly ApplicationDbContext _context;

    public ExamsController(IExamService examService, IGroupService groupService, IExportService exportService, ApplicationDbContext context)
    {
        _examService = examService;
        _groupService = groupService;
        _exportService = exportService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var exams = await _examService.GetAllAsync();
        var viewModel = exams.Select(e => new ExamListViewModel
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            GroupName = e.Group?.Name ?? "",
            GroupId = e.GroupId,
            ExamDate = e.ExamDate,
            MaxMarks = e.MaxMarks,
            PassMarks = e.PassMarks,
            ResultsCount = e.Results?.Count ?? 0,
            SuccessRate = e.Results?.Any() == true
                ? Math.Round((double)e.Results.Count(r => r.MarksObtained >= e.PassMarks) / e.Results.Count * 100, 1)
                : 0
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
        return View(new ExamCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExamCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
            return View(model);
        }

        var exam = new Exam
        {
            Title = model.Title,
            Description = model.Description,
            GroupId = model.GroupId,
            ExamDate = model.ExamDate,
            MaxMarks = model.MaxMarks,
            PassMarks = model.PassMarks
        };

        await _examService.CreateAsync(exam);
        TempData["Success"] = "تم إنشاء الامتحان بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EnterGrades(int id)
    {
        var exam = await _examService.GetExamWithResultsAsync(id);
        if (exam == null) return NotFound();

        ViewBag.ExamTitle = exam.Title;
        ViewBag.MaxMarks = exam.MaxMarks;
        ViewBag.PassMarks = exam.PassMarks;
        ViewBag.GroupId = exam.GroupId;
        ViewBag.ExamId = exam.Id;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetStudentForGrade(int examId, string code)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null) return Json(new { error = "الامتحان غير موجود" });

        var student = await _context.Students
            .Include(s => s.StudentGroups.Where(sg => sg.GroupId == exam.GroupId && sg.IsActive))
            .FirstOrDefaultAsync(s => s.StudentCode == code && !s.IsDeleted);

        if (student == null)
            return Json(new { error = "الطالب غير موجود في هذه المجموعة" });

        if (!student.StudentGroups.Any())
            return Json(new { error = "الطالب غير مسجل في هذه المجموعة" });

        var existing = await _context.ExamResults
            .FirstOrDefaultAsync(r => r.ExamId == examId && r.StudentId == student.Id);

        return Json(new
        {
            studentId = student.Id,
            studentCode = student.StudentCode,
            fullName = student.FullName,
            marksObtained = existing?.MarksObtained ?? 0,
            hasExisting = existing != null
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveStudentGrade(int examId, int studentId, int marksObtained, string? notes)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null) return Json(new { error = "الامتحان غير موجود" });

        if (marksObtained < 0 || marksObtained > exam.MaxMarks)
            return Json(new { error = $"الدرجة يجب أن تكون بين 0 و {exam.MaxMarks}" });

        var existing = await _context.ExamResults
            .FirstOrDefaultAsync(r => r.ExamId == examId && r.StudentId == studentId);

        if (existing != null)
        {
            existing.MarksObtained = marksObtained;
            existing.Notes = notes;
        }
        else
        {
            _context.ExamResults.Add(new ExamResult
            {
                ExamId = examId,
                StudentId = studentId,
                MarksObtained = marksObtained,
                Notes = notes
            });
        }

        await _context.SaveChangesAsync();

        var status = marksObtained >= exam.PassMarks ? "ناجح" : "راسب";
        return Json(new { success = true, message = $"تم حفظ درجة {marksObtained} للطالب", status });
    }

    public async Task<IActionResult> Results(int id)
    {
        var exam = await _examService.GetExamWithResultsAsync(id);
        if (exam == null) return NotFound();

        var model = new ExamResultsViewModel
        {
            ExamId = exam.Id,
            ExamTitle = exam.Title,
            MaxMarks = exam.MaxMarks,
            PassMarks = exam.PassMarks,
            Results = exam.Group?.StudentGroups?.Where(sg => sg.IsActive).Select(sg =>
            {
                var result = exam.Results?.FirstOrDefault(r => r.StudentId == sg.StudentId);
                return new ExamResultItemViewModel
                {
                    StudentId = sg.Student.Id,
                    StudentName = sg.Student.FullName,
                    StudentCode = sg.Student.StudentCode,
                    MarksObtained = result?.MarksObtained ?? 0,
                    IsPassed = result != null && result.MarksObtained >= exam.PassMarks
                };
            }).ToList() ?? new List<ExamResultItemViewModel>()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveResults(int examId, List<ExamResultItemViewModel> results)
    {
        var examResults = results.Select(r => new ExamResult
        {
            ExamId = examId,
            StudentId = r.StudentId,
            MarksObtained = r.MarksObtained,
            Notes = r.Notes
        }).ToList();

        await _examService.SaveResultsAsync(examId, examResults);
        TempData["Success"] = "تم حفظ النتائج بنجاح";
        return RedirectToAction(nameof(Results), new { id = examId });
    }

    public async Task<IActionResult> Ranking(int id)
    {
        var ranking = await _examService.GetStudentRankingAsync(id);
        ViewBag.ExamId = id;
        return View(ranking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _examService.DeleteAsync(id);
        TempData["Success"] = "تم حذف الامتحان بنجاح";
        return RedirectToAction(nameof(Index));
    }
}
