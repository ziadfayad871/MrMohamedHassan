using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class ApplicationRole : IdentityRole
{
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "الحالة")]
    public bool IsActive { get; set; } = true;
}
