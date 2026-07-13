using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class StudentListViewModel
{
    public int Id { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ParentName { get; set; }
    public string? School { get; set; }
    public string? Grade { get; set; }
    public Gender Gender { get; set; }
    public StudentStatus Status { get; set; }
    public DateTime JoinDate { get; set; }
    public string? ImageUrl { get; set; }
    public int GroupsCount { get; set; }
}

public class StudentCreateViewModel
{
    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(200)]
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "الهاتف")]
    public string? Phone { get; set; }

    [StringLength(200)]
    [Display(Name = "اسم ولي الأمر")]
    public string? ParentName { get; set; }

    [StringLength(20)]
    [Display(Name = "هاتف ولي الأمر")]
    public string? ParentPhone { get; set; }

    [StringLength(200)]
    [Display(Name = "المدرسة")]
    public string? School { get; set; }

    [StringLength(100)]
    [Display(Name = "المرحلة الدراسية")]
    public string? Grade { get; set; }

    [StringLength(500)]
    [Display(Name = "العنوان")]
    public string? Address { get; set; }

    [Display(Name = "تاريخ الميلاد")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "الجنس")]
    public Gender Gender { get; set; }

    [Display(Name = "تاريخ الالتحاق")]
    [DataType(DataType.Date)]
    public DateTime JoinDate { get; set; } = DateTime.Now;

    [Display(Name = "رسوم الاشتراك")]
    public decimal SubscriptionFee { get; set; }

    [Display(Name = "صورة الطالب")]
    public IFormFile? StudentImage { get; set; }

    [StringLength(1000)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    [Display(Name = "الحالة")]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    [Display(Name = "المجموعات")]
    public List<int> SelectedGroupIds { get; set; } = new();
}

public class StudentEditViewModel
{
    public int Id { get; set; }

    public string StudentCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(200)]
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "الهاتف")]
    public string? Phone { get; set; }

    [StringLength(200)]
    [Display(Name = "اسم ولي الأمر")]
    public string? ParentName { get; set; }

    [StringLength(20)]
    [Display(Name = "هاتف ولي الأمر")]
    public string? ParentPhone { get; set; }

    [StringLength(200)]
    [Display(Name = "المدرسة")]
    public string? School { get; set; }

    [StringLength(100)]
    [Display(Name = "المرحلة الدراسية")]
    public string? Grade { get; set; }

    [StringLength(500)]
    [Display(Name = "العنوان")]
    public string? Address { get; set; }

    [Display(Name = "تاريخ الميلاد")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "الجنس")]
    public Gender Gender { get; set; }

    [Display(Name = "تاريخ الالتحاق")]
    [DataType(DataType.Date)]
    public DateTime JoinDate { get; set; }

    [Display(Name = "رسوم الاشتراك")]
    public decimal SubscriptionFee { get; set; }

    [Display(Name = "صورة الطالب")]
    public IFormFile? StudentImage { get; set; }

    public string? ExistingImageUrl { get; set; }

    [StringLength(1000)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    [Display(Name = "الحالة")]
    public StudentStatus Status { get; set; }

    [Display(Name = "المجموعات")]
    public List<int> SelectedGroupIds { get; set; } = new();
}

public class StudentDetailsViewModel
{
    public int Id { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? School { get; set; }
    public string? Grade { get; set; }
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public DateTime JoinDate { get; set; }
    public decimal SubscriptionFee { get; set; }
    public string? ImageUrl { get; set; }
    public string? Notes { get; set; }
    public StudentStatus Status { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingBalance { get; set; }
    public List<string> Groups { get; set; } = new();
    public int AttendancePercentage { get; set; }
}

public class StudentSearchViewModel
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Grade { get; set; }
    public StudentStatus? Status { get; set; }
    public int? GroupId { get; set; }
}

public class StudentExamPerformanceViewModel
{
    public string ExamTitle { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int MaxMarks { get; set; }
    public int PassMarks { get; set; }
    public int? MarksObtained { get; set; }
    public bool HasResult { get; set; }
    public string Status { get; set; } = "لم يمتحن";
}
