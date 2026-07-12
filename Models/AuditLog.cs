using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.Models;

public class AuditLog
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    [Required]
    [StringLength(100)]
    public string Action { get; set; } = string.Empty;

    [StringLength(100)]
    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    [StringLength(2000)]
    public string? OldValues { get; set; }

    [StringLength(2000)]
    public string? NewValues { get; set; }

    [StringLength(500)]
    public string? IPAddress { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
