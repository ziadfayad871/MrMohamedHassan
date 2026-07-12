using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrMohamedHassan.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "الوصف")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "المبلغ")]
    public decimal Amount { get; set; }

    public int ExpenseCategoryId { get; set; }
    public virtual ExpenseCategory Category { get; set; } = null!;

    [Display(Name = "التاريخ")]
    public DateTime ExpenseDate { get; set; } = DateTime.Now;

    [StringLength(500)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    public string? CreatedById { get; set; }
    public virtual ApplicationUser? CreatedBy { get; set; }
}

public class ExpenseCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "اسم الفئة")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Display(Name = "نشط")]
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
