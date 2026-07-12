using FluentValidation;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Validators;

public class GroupCreateValidator : AbstractValidator<GroupCreateViewModel>
{
    public GroupCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم المجموعة مطلوب")
            .MaximumLength(200);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("السعة يجب أن تكون أكبر من صفر");

        RuleFor(x => x.Fee)
            .GreaterThanOrEqualTo(0).WithMessage("الرسوم لا يمكن أن تكون سالبة");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("يجب اختيار المعلم");
    }
}
