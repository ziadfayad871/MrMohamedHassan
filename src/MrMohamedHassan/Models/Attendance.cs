using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class Attendance
{
    [Key]
    public int Id { get; set; }

    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;

    [Display(Name = "التاريخ")]
    public DateTime Date { get; set; } = DateTime.Today;

    [Display(Name = "الحالة")]
    public AttendanceStatus Status { get; set; }

    [Display(Name = "وقت الحضور")]
    public DateTime? CheckInTime { get; set; }

    [StringLength(500)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    [Display(Name = "سجل بواسطة")]
    public string? RecordedById { get; set; }

    public virtual ApplicationUser? RecordedBy { get; set; }
}

public enum AttendanceStatus
{
    [Display(Name = "حاضر")]
    Present,
    [Display(Name = "غائب")]
    Absent,
    [Display(Name = "متأخر")]
    Late
}
