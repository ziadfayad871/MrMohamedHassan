using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrMohamedHassan.Models;

public class Teacher
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "الهاتف")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "البريد الإلكتروني")]
    public string? Email { get; set; }

    [StringLength(100)]
    [Display(Name = "التخصص")]
    public string? Specialization { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "الراتب")]
    public decimal Salary { get; set; }

    [Display(Name = "صورة المعلم")]
    public string? ImageUrl { get; set; }

    [Display(Name = "الحالة")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "حذف ناعم")]
    public bool IsDeleted { get; set; } = false;

    [Display(Name = "تاريخ الالتحاق")]
    public DateTime JoinDate { get; set; } = DateTime.Now;

    public string? UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
