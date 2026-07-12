using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class ExpenseListViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int ExpenseCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public string? Notes { get; set; }
}

public class ExpenseCreateViewModel
{
    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(200)]
    [Display(Name = "الوصف")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "المبلغ مطلوب")]
    [Display(Name = "المبلغ")]
    public decimal Amount { get; set; }

    [Required]
    [Display(Name = "الفئة")]
    public int ExpenseCategoryId { get; set; }

    [Display(Name = "التاريخ")]
    public DateTime ExpenseDate { get; set; } = DateTime.Now;

    [StringLength(500)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }
}
