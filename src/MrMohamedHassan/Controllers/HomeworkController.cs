using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;
using System.Security.Claims;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class HomeworkController : Controller
{
    private readonly IHomeworkService _homeworkService;
    private readonly IGroupService _groupService;
    private readonly IFileService _fileService;

    public HomeworkController(IHomeworkService homeworkService, IGroupService groupService, IFileService fileService)
    {
        _homeworkService = homeworkService;
        _groupService = groupService;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var homeworks = await _homeworkService.GetAllAsync();
        var viewModel = homeworks.Select(h => new HomeworkListViewModel
        {
            Id = h.Id,
            Title = h.Title,
            Description = h.Description,
            GroupName = h.Group?.Name ?? "",
            GroupId = h.GroupId,
            DueDate = h.DueDate,
            MaxMarks = h.MaxMarks,
            SubmissionsCount = h.Submissions?.Count ?? 0,
            TotalStudents = h.Group?.StudentGroups?.Count(sg => sg.IsActive) ?? 0,
            CreatedAt = h.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
        return View(new HomeworkCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HomeworkCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
            return View(model);
        }

        var homework = new Homework
        {
            Title = model.Title,
            Description = model.Description,
            GroupId = model.GroupId,
            DueDate = model.DueDate,
            MaxMarks = model.MaxMarks,
            CreatedById = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
        };

        if (model.Attachment != null)
            homework.AttachmentUrl = await _fileService.UploadFileAsync(model.Attachment, "homework");

        await _homeworkService.CreateAsync(homework);
        TempData["Success"] = "تم إنشاء الواجب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Submissions(int id)
    {
        var homework = await _homeworkService.GetHomeworkWithSubmissionsAsync(id);
        if (homework == null) return NotFound();

        ViewBag.HomeworkTitle = homework.Title;
        ViewBag.MaxMarks = homework.MaxMarks;

        var submissions = homework.Submissions.Select(s => new HomeworkSubmissionViewModel
        {
            HomeworkId = id,
            HomeworkTitle = homework.Title,
            StudentId = s.StudentId,
            StudentName = s.Student?.FullName ?? "",
            SubmittedAt = s.SubmittedAt,
            AttachmentUrl = s.AttachmentUrl,
            Marks = s.Marks,
            TeacherComments = s.TeacherComments,
            IsGraded = s.IsGraded,
            StudentComment = s.StudentComment
        }).ToList();

        return View(submissions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Grade(int submissionId, int marks, string? comments, int homeworkId)
    {
        await _homeworkService.GradeAsync(submissionId, marks, comments);
        TempData["Success"] = "تم التصحيح بنجاح";
        return RedirectToAction(nameof(Submissions), new { id = homeworkId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _homeworkService.DeleteAsync(id);
        TempData["Success"] = "تم حذف الواجب بنجاح";
        return RedirectToAction(nameof(Index));
    }
}
