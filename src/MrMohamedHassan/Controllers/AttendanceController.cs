using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;
using System.Security.Claims;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;
    private readonly IGroupService _groupService;
    private readonly ApplicationDbContext _context;

    public AttendanceController(IAttendanceService attendanceService, IGroupService groupService, ApplicationDbContext context)
    {
        _attendanceService = attendanceService;
        _groupService = groupService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
        return View();
    }

    public async Task<IActionResult> TakeAttendance(int? groupId, DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;
        var dayNames = new[] { "الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت" };
        var dayName = dayNames[(int)selectedDate.DayOfWeek];
        var allGroups = await _groupService.GetActiveGroupsAsync();
        var filtered = allGroups.Where(g =>
            string.IsNullOrEmpty(g.ScheduleDays) ||
            g.ScheduleDays.Split(',').Select(d => d.Trim()).Contains(dayName)
        ).ToList();
        ViewBag.Groups = filtered;
        ViewBag.SelectedDate = selectedDate;

        if (groupId == null)
            return View(new AttendanceTakeViewModel { Date = selectedDate });

        var students = await _context.Students
            .Where(s => s.StudentGroups.Any(sg => sg.GroupId == groupId && sg.IsActive) && !s.IsDeleted)
            .ToListAsync();

        var existingAttendance = await _attendanceService.GetAttendanceByGroupAndDateAsync(groupId.Value, selectedDate);
        var attendanceList = students.Select(s =>
        {
            var existing = existingAttendance.FirstOrDefault(a => a.StudentId == s.Id);
            return new AttendanceItemViewModel
            {
                StudentId = s.Id,
                StudentName = s.FullName,
                StudentCode = s.StudentCode,
                Status = existing?.Status ?? AttendanceStatus.Present,
                Notes = existing?.Notes
            };
        }).ToList();

        var model = new AttendanceTakeViewModel
        {
            GroupId = groupId.Value,
            Date = selectedDate,
            Students = attendanceList
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetStudentInfo(string code, int groupId)
    {
        var student = await _context.Students
            .Include(s => s.StudentGroups.Where(sg => sg.GroupId == groupId))
            .ThenInclude(sg => sg.Group)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.StudentCode == code && !s.IsDeleted);

        if (student == null)
            return Json(new { error = "الطالب غير موجود" });

        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var existingAtt = await _context.Attendances
            .FirstOrDefaultAsync(a => a.StudentId == student.Id && a.GroupId == groupId && a.Date.Date == today);

        var monthlyPayment = student.Payments
            .Where(p => p.PaymentType == PaymentType.Subscription && p.PaymentDate >= monthStart && p.PaymentDate < monthStart.AddMonths(1))
            .OrderByDescending(p => p.PaymentDate)
            .FirstOrDefault();

        var groupName = student.StudentGroups.FirstOrDefault()?.Group?.Name ?? "";

        return Json(new StudentAttendanceInfo
        {
            StudentId = student.Id,
            StudentCode = student.StudentCode,
            FullName = student.FullName,
            Phone = student.Phone,
            GroupName = groupName,
            GroupId = groupId,
            TodayAttendance = existingAtt?.Status.ToString(),
            MonthlyPaid = monthlyPayment != null,
            LastPaymentDate = monthlyPayment?.PaymentDate,
            SubscriptionFee = student.SubscriptionFee
        });
    }

    [HttpPost]
    public async Task<IActionResult> RecordAttendance([FromBody] RecordAttendanceRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.Today;

        var existing = await _context.Attendances
            .FirstOrDefaultAsync(a => a.StudentId == request.StudentId && a.GroupId == request.GroupId && a.Date.Date == today);

        if (existing != null)
        {
            existing.Status = Enum.Parse<AttendanceStatus>(request.Status);
            existing.CheckInTime = request.Status == "Present" || request.Status == "Late" ? DateTime.Now : null;
        }
        else
        {
            _context.Attendances.Add(new Attendance
            {
                StudentId = request.StudentId,
                GroupId = request.GroupId,
                Date = today,
                Status = Enum.Parse<AttendanceStatus>(request.Status),
                CheckInTime = request.Status == "Present" || request.Status == "Late" ? DateTime.Now : null,
                RecordedById = userId
            });
        }

        await _context.SaveChangesAsync();
        return Json(new { success = true, status = request.Status });
    }

    public async Task<IActionResult> RecordAbsentees(int groupId, DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var students = await _context.Students
            .Where(s => s.StudentGroups.Any(sg => sg.GroupId == groupId && sg.IsActive) && !s.IsDeleted)
            .ToListAsync();

        var existingIds = await _context.Attendances
            .Where(a => a.GroupId == groupId && a.Date.Date == selectedDate.Date)
            .Select(a => a.StudentId)
            .ToListAsync();

        var absentees = students.Where(s => !existingIds.Contains(s.Id)).ToList();
        foreach (var student in absentees)
        {
            _context.Attendances.Add(new Attendance
            {
                StudentId = student.Id,
                GroupId = groupId,
                Date = selectedDate,
                Status = AttendanceStatus.Absent,
                RecordedById = userId
            });
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = $"تم تسجيل غياب {absentees.Count} طالب";
        return RedirectToAction(nameof(TakeAttendance), new { groupId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TakeAttendance(AttendanceTakeViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var attendanceList = model.Students.Select(s => new Attendance
        {
            StudentId = s.StudentId,
            GroupId = model.GroupId,
            Date = model.Date,
            Status = s.Status,
            Notes = s.Notes,
            RecordedById = userId
        }).ToList();

        await _attendanceService.SaveAttendanceAsync(attendanceList);
        TempData["Success"] = "تم حفظ الحضور بنجاح";
        return RedirectToAction(nameof(TakeAttendance), new { groupId = model.GroupId, date = model.Date.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> DailyReport(int? groupId, DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;
        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();

        if (groupId == null)
            return View(new List<Attendance>());

        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Group)
            .Where(a => a.GroupId == groupId && a.Date.Date == selectedDate.Date)
            .ToListAsync();

        ViewBag.SelectedGroup = groupId;
        ViewBag.SelectedDate = selectedDate;
        return View(attendance);
    }
}
