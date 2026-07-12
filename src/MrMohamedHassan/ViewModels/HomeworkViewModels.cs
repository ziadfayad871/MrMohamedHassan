using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class HomeworkListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxMarks { get; set; }
    public int SubmissionsCount { get; set; }
    public int TotalStudents { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HomeworkCreateViewModel
{
    [Required(ErrorMessage = "عنوان الواجب مطلوب")]
    [StringLength(200)]
    [Display(Name = "عنوان الواجب")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "المجموعة")]
    public int GroupId { get; set; }

    [Required]
    [Display(Name = "تاريخ التسليم")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);

    [Display(Name = "الدرجة الكاملة")]
    public int MaxMarks { get; set; } = 100;

    [Display(Name = "ملف مرفق")]
    public IFormFile? Attachment { get; set; }
}

public class HomeworkSubmissionViewModel
{
    public int HomeworkId { get; set; }
    public string HomeworkTitle { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string? AttachmentUrl { get; set; }
    public int? Marks { get; set; }
    public string? TeacherComments { get; set; }
    public bool IsGraded { get; set; }
    public string? StudentComment { get; set; }
}

public class HomeworkGradeViewModel
{
    public int SubmissionId { get; set; }
    public int MaxMarks { get; set; }

    [Required]
    [Display(Name = "الدرجة")]
    public int Marks { get; set; }

    [Display(Name = "ملاحظات المعلم")]
    public string? Comments { get; set; }
}
