using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class NotificationListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class NotificationCreateViewModel
{
    [Required(ErrorMessage = "العنوان مطلوب")]
    [StringLength(200)]
    [Display(Name = "العنوان")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "المحتوى مطلوب")]
    [StringLength(2000)]
    [Display(Name = "المحتوى")]
    public string Message { get; set; } = string.Empty;

    [Required]
    [Display(Name = "النوع")]
    public NotificationType Type { get; set; }

    [Display(Name = "مرسل إلى")]
    public string? TargetUserId { get; set; }
}
