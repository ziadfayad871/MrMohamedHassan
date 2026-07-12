using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class Setting
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Key { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Value { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Display(Name = "تاريخ التحديث")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
