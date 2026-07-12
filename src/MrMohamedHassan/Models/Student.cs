using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrMohamedHassan.Models;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "كود الطالب")]
    public string StudentCode { get; set; } = string.Empty;

    [Required]
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

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "رسوم الاشتراك")]
    public decimal SubscriptionFee { get; set; }

    [Display(Name = "صورة الطالب")]
    public string? ImageUrl { get; set; }

    [StringLength(1000)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    [Display(Name = "الحالة")]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    [Display(Name = "حذف ناعم")]
    public bool IsDeleted { get; set; } = false;

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "تاريخ التحديث")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    public virtual ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; } = new List<HomeworkSubmission>();
}

public enum Gender
{
    [Display(Name = "ذكر")]
    Male,
    [Display(Name = "أنثى")]
    Female
}

public enum StudentStatus
{
    [Display(Name = "نشط")]
    Active,
    [Display(Name = "غير نشط")]
    Inactive,
    [Display(Name = "منسحب")]
    Withdrawn,
    [Display(Name = "متخرج")]
    Graduated
}
