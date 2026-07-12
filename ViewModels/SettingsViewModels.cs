using System.ComponentModel.DataAnnotations;

namespace MrMohamedHassan.ViewModels;

public class SettingsViewModel
{
    [Display(Name = "اسم المركز")]
    public string? CenterName { get; set; }

    [Display(Name = "هاتف المركز")]
    public string? CenterPhone { get; set; }

    [Display(Name = "البريد الإلكتروني")]
    public string? CenterEmail { get; set; }

    [Display(Name = "العنوان")]
    public string? CenterAddress { get; set; }

    [Display(Name = "السنة الدراسية")]
    public string? AcademicYear { get; set; }

    [Display(Name = "شعار المركز")]
    public string? CenterLogo { get; set; }

    [Display(Name = "رفع شعار")]
    public IFormFile? LogoFile { get; set; }

    [Display(Name = "العملة")]
    public string? Currency { get; set; }
}
