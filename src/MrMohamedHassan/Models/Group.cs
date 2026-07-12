using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class Group
{
    [Key]
    public int Id { get; set; }

    [Required]
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
    public TimeSpan? StartTime { get; set; }

    [Display(Name = "وقت النهاية")]
    public TimeSpan? EndTime { get; set; }

    [Display(Name = "الرسوم")]
    public decimal Fee { get; set; }

    [Display(Name = "الحالة")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "حذف ناعم")]
    public bool IsDeleted { get; set; } = false;

    [Display(Name = "السنة الدراسية")]
    public string? AcademicYear { get; set; }

    public int TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();
}
