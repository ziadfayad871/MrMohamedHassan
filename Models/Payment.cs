using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MrMohamedHassan.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "المبلغ")]
    public decimal Amount { get; set; }

    [Display(Name = "نوع الدفعة")]
    public PaymentType PaymentType { get; set; }

    [Display(Name = "طريقة الدفع")]
    public PaymentMethod PaymentMethod { get; set; }

    [Display(Name = "التاريخ")]
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "الخصم")]
    public decimal Discount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "المبلغ المدفوع")]
    public decimal PaidAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "المتبقي")]
    public decimal Remaining { get; set; }

    [StringLength(200)]
    [Display(Name = "رقم الإيصال")]
    public string? ReceiptNumber { get; set; }

    [StringLength(500)]
    [Display(Name = "ملاحظات")]
    public string? Notes { get; set; }

    [Display(Name = "مدفوع")]
    public bool IsPaid { get; set; } = true;

    public string? CreatedById { get; set; }
    public virtual ApplicationUser? CreatedBy { get; set; }
}

public enum PaymentType
{
    [Display(Name = "اشتراك")]
    Subscription,
    [Display(Name = "دفعة جزئية")]
    Partial,
    [Display(Name = "رسوم إضافية")]
    Extra
}

public enum PaymentMethod
{
    [Display(Name = "نقدي")]
    Cash,
    [Display(Name = "تحويل بنكي")]
    BankTransfer,
    [Display(Name = "فودافون كاش")]
    VodafoneCash,
    [Display(Name = "الدفع الإلكتروني")]
    Online
}
