using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class GroupsController : Controller
{
    private readonly IGroupService _groupService;
    private readonly ApplicationDbContext _context;

    public GroupsController(IGroupService groupService, ApplicationDbContext context)
    {
        _groupService = groupService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _groupService.GetAllAsync();
        var viewModel = groups.Select(g => new GroupListViewModel
        {
            Id = g.Id,
            Name = g.Name,
            Subject = g.Subject,
            TeacherName = g.Teacher?.FullName ?? "",
            Capacity = g.Capacity,
            ScheduleDays = g.ScheduleDays,
            StartTime = g.StartTime,
            EndTime = g.EndTime,
            Fee = g.Fee,
            IsActive = g.IsActive
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var defaultTeacher = await _context.Teachers.Where(t => t.IsActive && !t.IsDeleted).FirstOrDefaultAsync();
        var model = new GroupCreateViewModel();
        if (defaultTeacher != null)
            model.TeacherId = defaultTeacher.Id;
        model.Subject = "دراسات اجتماعيه";
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GroupCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == model.TeacherId && t.IsActive && !t.IsDeleted);
        if (!teacherExists)
        {
            ModelState.AddModelError("", "المعلم غير موجود. الرجاء إضافة معلم أولاً.");
            return View(model);
        }

        var group = new Group
        {
            Name = model.Name,
            Subject = "دراسات اجتماعيه",
            Capacity = model.Capacity,
            Grade = model.Grade,
            ScheduleDays = model.ScheduleDays,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Fee = model.Fee,
            TeacherId = model.TeacherId,
            AcademicYear = model.AcademicYear
        };

        await _groupService.CreateAsync(group);

        if (model.SelectedStudentIds.Any())
            await _groupService.AssignStudentsAsync(group.Id, model.SelectedStudentIds);

        TempData["Success"] = "تم إنشاء المجموعة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var group = await _groupService.GetGroupWithDetailsAsync(id);
        if (group == null) return NotFound();

        var model = new GroupEditViewModel
        {
            Id = group.Id,
            Name = group.Name,
            Subject = "دراسات اجتماعيه",
            Capacity = group.Capacity,
            Grade = group.Grade,
            ScheduleDays = group.ScheduleDays,
            StartTime = group.StartTime,
            EndTime = group.EndTime,
            Fee = group.Fee,
            TeacherId = group.TeacherId,
            IsActive = group.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(GroupEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var group = await _groupService.GetByIdAsync(model.Id);
        if (group == null) return NotFound();

        group.Name = model.Name;
            group.Subject = "دراسات اجتماعيه";
        group.Capacity = model.Capacity;
        group.Grade = model.Grade;
        group.ScheduleDays = model.ScheduleDays;
        group.StartTime = model.StartTime;
        group.EndTime = model.EndTime;
        group.Fee = model.Fee;
        group.TeacherId = model.TeacherId;
        group.IsActive = model.IsActive;

        await _groupService.UpdateAsync(group);
        TempData["Success"] = "تم تحديث المجموعة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var group = await _groupService.GetGroupWithDetailsAsync(id);
        if (group == null) return NotFound();

        var viewModel = new GroupDetailsViewModel
        {
            Id = group.Id,
            Name = group.Name,
            Subject = group.Subject,
            TeacherName = group.Teacher?.FullName ?? "",
            Capacity = group.Capacity,
            ScheduleDays = group.ScheduleDays,
            StartTime = group.StartTime,
            EndTime = group.EndTime,
            Fee = group.Fee,
            IsActive = group.IsActive,
            Students = group.StudentGroups
                .Where(sg => sg.IsActive)
                .Select(sg => new StudentListViewModel
                {
                    Id = sg.Student.Id,
                    StudentCode = sg.Student.StudentCode,
                    FullName = sg.Student.FullName,
                    Phone = sg.Student.Phone
                }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _groupService.SoftDeleteAsync(id);
        TempData["Success"] = "تم حذف المجموعة بنجاح";
        return RedirectToAction(nameof(Index));
    }
}
