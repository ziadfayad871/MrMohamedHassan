using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class ExamListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public DateTime ExamDate { get; set; }
    public int MaxMarks { get; set; }
    public int PassMarks { get; set; }
    public int ResultsCount { get; set; }
    public double SuccessRate { get; set; }
}

public class ExamCreateViewModel
{
    [Required(ErrorMessage = "اسم الامتحان مطلوب")]
    [StringLength(200)]
    [Display(Name = "اسم الامتحان")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "المجموعة")]
    public int GroupId { get; set; }

    [Display(Name = "التاريخ")]
    public DateTime ExamDate { get; set; } = DateTime.Now;

    [Display(Name = "الدرجة النهائية")]
    public int MaxMarks { get; set; } = 100;

    [Display(Name = "درجة النجاح")]
    public int PassMarks { get; set; } = 50;
}

public class ExamResultsViewModel
{
    public int ExamId { get; set; }
    public string ExamTitle { get; set; } = string.Empty;
    public int MaxMarks { get; set; }
    public int PassMarks { get; set; }
    public List<ExamResultItemViewModel> Results { get; set; } = new();
}

public class ExamResultItemViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public int MarksObtained { get; set; }
    public bool IsPassed { get; set; }
    public string? Notes { get; set; }
}
