using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrMohamedHassan.Models;

public class Exam
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "اسم الامتحان")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;

    [Display(Name = "التاريخ")]
    public DateTime ExamDate { get; set; } = DateTime.Now;

    [Display(Name = "الدرجة النهائية")]
    public int MaxMarks { get; set; } = 100;

    [Display(Name = "درجة النجاح")]
    public int PassMarks { get; set; } = 50;

    public virtual ICollection<ExamResult> Results { get; set; } = new List<ExamResult>();
}

public class ExamResult
{
    [Key]
    public int Id { get; set; }

    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; } = null!;

    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    [Display(Name = "الدرجة")]
    public int MarksObtained { get; set; }

    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }
}
