using FluentValidation;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Validators;

public class PaymentCreateValidator : AbstractValidator<PaymentCreateViewModel>
{
    public PaymentCreateValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("المبلغ يجب أن يكون أكبر من صفر");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("الخصم لا يمكن أن يكون سالباً")
            .LessThanOrEqualTo(x => x.Amount).WithMessage("الخصم لا يمكن أن يكون أكبر من المبلغ");
    }
}
