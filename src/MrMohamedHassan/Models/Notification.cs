using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "العنوان")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    [Display(Name = "المحتوى")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "النوع")]
    public NotificationType Type { get; set; }

    [Display(Name = "مرسل إلى")]
    public string? TargetUserId { get; set; }

    [Display(Name = "مرسل")]
    public string? SenderId { get; set; }

    [Display(Name = "مقروء")]
    public bool IsRead { get; set; } = false;

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser? TargetUser { get; set; }
    public virtual ApplicationUser? Sender { get; set; }
}

public enum NotificationType
{
    [Display(Name = "عام")]
    General,
    [Display(Name = "دفعة")]
    Payment,
    [Display(Name = "حضور")]
    Attendance,
    [Display(Name = "امتحان")]
    Exam,
    [Display(Name = "واجب")]
    Homework,
    [Display(Name = "تنبيه")]
    Alert
}
