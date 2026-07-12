using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.ViewModels;

public class GroupListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public string? ScheduleDays { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public decimal Fee { get; set; }
    public bool IsActive { get; set; }
}

public class GroupCreateViewModel
{
    [Required(ErrorMessage = "اسم المجموعة مطلوب")]
    [StringLength(200)]
    [Display(Name = "اسم المجموعة")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "المادة")]
    public string? Subject { get; set; }

    [Display(Name = "السعة القصوى")]
    public int Capacity { get; set; } = 30;

    [Display(Name = "المرحلة الدراسية")]
    public string? Grade { get; set; }

    [Display(Name = "الأيام")]
    public string? ScheduleDays { get; set; }

    [Display(Name = "وقت البداية")]
    [DataType(DataType.Time)]
    public TimeSpan? StartTime { get; set; }

    [Display(Name = "وقت النهاية")]
    [DataType(DataType.Time)]
    public TimeSpan? EndTime { get; set; }

    [Display(Name = "الرسوم")]
    public decimal Fee { get; set; }

    [Required(ErrorMessage = "يجب اختيار المعلم")]
    [Display(Name = "المعلم")]
    public int TeacherId { get; set; }

    [Display(Name = "السنة الدراسية")]
    public string? AcademicYear { get; set; }

    public List<int> SelectedStudentIds { get; set; } = new();
}

public class GroupEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المجموعة مطلوب")]
    [StringLength(200)]
    [Display(Name = "اسم المجموعة")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "المادة")]
    public string? Subject { get; set; }

    [Display(Name = "السعة القصوى")]
    public int Capacity { get; set; }

    [Display(Name = "المرحلة الدراسية")]
    public string? Grade { get; set; }

    [Display(Name = "الأيام")]
    public string? ScheduleDays { get; set; }

    [DataType(DataType.Time)]
    public TimeSpan? StartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeSpan? EndTime { get; set; }

    [Display(Name = "الرسوم")]
    public decimal Fee { get; set; }

    [Required]
    [Display(Name = "المعلم")]
    public int TeacherId { get; set; }

    public bool IsActive { get; set; }
}

public class GroupDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? ScheduleDays { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public decimal Fee { get; set; }
    public bool IsActive { get; set; }
    public List<StudentListViewModel> Students { get; set; } = new();
}
