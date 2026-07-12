using FluentValidation;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Validators;

public class StudentCreateValidator : AbstractValidator<StudentCreateViewModel>
{
    public StudentCreateValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("الاسم الكامل مطلوب")
            .MaximumLength(200).WithMessage("الاسم لا يزيد عن 200 حرف");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("رقم الهاتف لا يزيد عن 20 رقم")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.ParentPhone)
            .MaximumLength(20).WithMessage("رقم هاتف ولي الأمر لا يزيد عن 20 رقم")
            .When(x => !string.IsNullOrEmpty(x.ParentPhone));

        RuleFor(x => x.SubscriptionFee)
            .GreaterThanOrEqualTo(0).WithMessage("رسوم الاشتراك يجب أن تكون أكبر من أو تساوي صفر");
    }
}

public class StudentEditValidator : AbstractValidator<StudentEditViewModel>
{
    public StudentEditValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("الاسم الكامل مطلوب")
            .MaximumLength(200);

        RuleFor(x => x.SubscriptionFee)
            .GreaterThanOrEqualTo(0);
    }
}
