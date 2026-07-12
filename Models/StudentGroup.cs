using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class StudentGroup
{
    [Key]
    public int Id { get; set; }

    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;

    [Display(Name = "تاريخ الانضمام")]
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "نشط")]
    public bool IsActive { get; set; } = true;
}
