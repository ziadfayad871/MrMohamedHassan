using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class ApplicationUser : IdentityUser
{
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "صورة الملف الشخصي")]
    public string? ProfileImageUrl { get; set; }

    [Display(Name = "المسمى الوظيفي")]
    public string? JobTitle { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "الحالة")]
    public bool IsActive { get; set; } = true;

    public string? CreatedById { get; set; }

    public virtual ApplicationUser? CreatedBy { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
