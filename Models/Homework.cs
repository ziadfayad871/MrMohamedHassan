using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class Homework
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "عنوان الواجب")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;

    [Display(Name = "تاريخ التسليم")]
    public DateTime DueDate { get; set; }

    [Display(Name = "الدرجة الكاملة")]
    public int MaxMarks { get; set; } = 100;

    [Display(Name = "الملف المرفق")]
    public string? AttachmentUrl { get; set; }

    [Display(Name = "أنشأه")]
    public string? CreatedById { get; set; }
    public virtual ApplicationUser? CreatedBy { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<HomeworkSubmission> Submissions { get; set; } = new List<HomeworkSubmission>();
}

public class HomeworkSubmission
{
    [Key]
    public int Id { get; set; }

    public int HomeworkId { get; set; }
    public virtual Homework Homework { get; set; } = null!;

    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    [Display(Name = "تاريخ التسليم")]
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "الملف المرفق")]
    public string? AttachmentUrl { get; set; }

    [Display(Name = "الدرجة")]
    public int? Marks { get; set; }

    [StringLength(500)]
    [Display(Name = "ملاحظات المعلم")]
    public string? TeacherComments { get; set; }

    [Display(Name = "تم التصحيح")]
    public bool IsGraded { get; set; } = false;

    [StringLength(1000)]
    [Display(Name = "تعليق الطالب")]
    public string? StudentComment { get; set; }
}
