using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class PaymentListViewModel
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidAmount { get; set; }
    public PaymentType PaymentType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? Notes { get; set; }
}

public class PaymentCreateViewModel
{
    [Required]
    [Display(Name = "الطالب")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "المبلغ مطلوب")]
    [Display(Name = "المبلغ")]
    public decimal Amount { get; set; }

    [Display(Name = "الخصم")]
    public decimal Discount { get; set; }

    [Required]
    [Display(Name = "نوع الدفعة")]
    public PaymentType PaymentType { get; set; }

    [Required]
    [Display(Name = "طريقة الدفع")]
    public PaymentMethod PaymentMethod { get; set; }

    [Display(Name = "التاريخ")]
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [StringLength(500)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }
}

public class PaymentReceiptViewModel
{
    public int PaymentId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string CenterName { get; set; } = string.Empty;
    public string CenterPhone { get; set; } = string.Empty;
}
