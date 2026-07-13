using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class StudentsController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;
    private readonly IPaymentService _paymentService;
    private readonly IFileService _fileService;
    private readonly IQrCodeService _qrCodeService;
    private readonly ApplicationDbContext _context;

    public StudentsController(
        IStudentService studentService,
        IGroupService groupService,
        IPaymentService paymentService,
        IFileService fileService,
        IQrCodeService qrCodeService,
        ApplicationDbContext context)
    {
        _studentService = studentService;
        _groupService = groupService;
        _paymentService = paymentService;
        _fileService = fileService;
        _qrCodeService = qrCodeService;
        _context = context;
    }

    public async Task<IActionResult> Index(string? name, string? code, StudentStatus? status, int page = 1)
    {
        var students = await _studentService.SearchAsync(name, code, status);
        var pageSize = 10;
        var totalCount = students.Count();
        var pagedStudents = students.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModels = pagedStudents.Select(s => new StudentListViewModel
        {
            Id = s.Id,
            StudentCode = s.StudentCode,
            FullName = s.FullName,
            Phone = s.Phone,
            ParentName = s.ParentName,
            School = s.School,
            Grade = s.Grade,
            Gender = s.Gender,
            Status = s.Status,
            JoinDate = s.JoinDate,
            ImageUrl = s.ImageUrl,
            GroupsCount = s.StudentGroups?.Count(sg => sg.IsActive) ?? 0
        }).ToList();

        ViewBag.TotalCount = totalCount;
        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.Name = name;
        ViewBag.Code = code;
        ViewBag.StatusFilter = status;

        return View(viewModels);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
        return View(new StudentCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
            return View(model);
        }

        var primaryGroupId = model.SelectedGroupIds.FirstOrDefault();
        var student = new Student
        {
            StudentCode = await _studentService.GenerateStudentCodeAsync(primaryGroupId > 0 ? primaryGroupId : null),
            FullName = model.FullName,
            Phone = model.Phone,
            ParentName = model.ParentName,
            ParentPhone = model.ParentPhone,
            School = model.School,
            Grade = model.Grade,
            Address = model.Address,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            JoinDate = model.JoinDate,
            SubscriptionFee = model.SubscriptionFee,
            Notes = model.Notes,
            Status = model.Status
        };

        if (model.StudentImage != null)
            student.ImageUrl = await _fileService.UploadImageAsync(model.StudentImage, "students");

        var created = await _studentService.CreateAsync(student);

        if (model.SelectedGroupIds.Any())
        {
            foreach (var groupId in model.SelectedGroupIds)
            {
                _context.StudentGroups.Add(new StudentGroup
                {
                    StudentId = created.Id,
                    GroupId = groupId,
                    IsActive = true,
                    EnrolledAt = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();
        }

        TempData["Success"] = "تم إضافة الطالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentService.GetStudentWithDetailsAsync(id);
        if (student == null) return NotFound();

        var model = new StudentEditViewModel
        {
            Id = student.Id,
            StudentCode = student.StudentCode,
            FullName = student.FullName,
            Phone = student.Phone,
            ParentName = student.ParentName,
            ParentPhone = student.ParentPhone,
            School = student.School,
            Grade = student.Grade,
            Address = student.Address,
            BirthDate = student.BirthDate,
            Gender = student.Gender,
            JoinDate = student.JoinDate,
            SubscriptionFee = student.SubscriptionFee,
            Notes = student.Notes,
            Status = student.Status,
            ExistingImageUrl = student.ImageUrl,
            SelectedGroupIds = student.StudentGroups.Where(sg => sg.IsActive).Select(sg => sg.GroupId).ToList()
        };

        ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Groups = await _groupService.GetActiveGroupsAsync();
            return View(model);
        }

        var student = await _studentService.GetByIdAsync(model.Id);
        if (student == null) return NotFound();

        student.FullName = model.FullName;
        student.Phone = model.Phone;
        student.ParentName = model.ParentName;
        student.ParentPhone = model.ParentPhone;
        student.School = model.School;
        student.Grade = model.Grade;
        student.Address = model.Address;
        student.BirthDate = model.BirthDate;
        student.Gender = model.Gender;
        student.JoinDate = model.JoinDate;
        student.SubscriptionFee = model.SubscriptionFee;
        student.Notes = model.Notes;
        student.Status = model.Status;

        if (model.StudentImage != null)
        {
            if (!string.IsNullOrEmpty(student.ImageUrl))
                _fileService.DeleteFile(student.ImageUrl);
            student.ImageUrl = await _fileService.UploadImageAsync(model.StudentImage, "students");
        }

        await _studentService.UpdateAsync(student);
        TempData["Success"] = "تم تحديث بيانات الطالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentService.GetStudentWithDetailsAsync(id);
        if (student == null) return NotFound();

        var totalPaid = await _paymentService.GetStudentTotalPaymentsAsync(id);

        var viewModel = new StudentDetailsViewModel
        {
            Id = student.Id,
            StudentCode = student.StudentCode,
            FullName = student.FullName,
            Phone = student.Phone,
            ParentName = student.ParentName,
            ParentPhone = student.ParentPhone,
            School = student.School,
            Grade = student.Grade,
            Address = student.Address,
            BirthDate = student.BirthDate,
            Gender = student.Gender,
            JoinDate = student.JoinDate,
            SubscriptionFee = student.SubscriptionFee,
            ImageUrl = student.ImageUrl,
            Notes = student.Notes,
            Status = student.Status,
            TotalPaid = totalPaid,
            RemainingBalance = student.SubscriptionFee - totalPaid,
            Groups = student.StudentGroups.Where(sg => sg.IsActive).Select(sg => sg.Group.Name).ToList(),
            AttendancePercentage = student.Attendances.Count > 0
                ? (int)((double)student.Attendances.Count(a => a.Status == AttendanceStatus.Present) / student.Attendances.Count * 100)
                : 0
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _studentService.SoftDeleteAsync(id);
        TempData["Success"] = "تم حذف الطالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> QrCode(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null) return NotFound();

        var qrData = $"STUDENT:{student.Id}:{student.StudentCode}:{student.FullName}";
        var qrBase64 = _qrCodeService.GetQrCodeAsBase64(qrData);

        ViewBag.Student = student;
        ViewBag.QrCode = qrBase64;
        return View();
    }

    public async Task<IActionResult> PaymentHistory(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null) return NotFound();

        var payments = await _paymentService.GetPaymentsByStudentAsync(id);
        ViewBag.Student = student;
        return View(payments);
    }

    public async Task<IActionResult> AttendanceHistory(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null) return NotFound();

        var attendance = await _context.Attendances
            .Include(a => a.Group)
            .Where(a => a.StudentId == id)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        ViewBag.Student = student;
        return View(attendance);
    }
}
