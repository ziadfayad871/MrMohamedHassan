using FluentValidation;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Validators;

public class ExamCreateValidator : AbstractValidator<ExamCreateViewModel>
{
    public ExamCreateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("اسم الامتحان مطلوب")
            .MaximumLength(200);

        RuleFor(x => x.GroupId)
            .GreaterThan(0).WithMessage("يجب اختيار المجموعة");

        RuleFor(x => x.MaxMarks)
            .GreaterThan(0).WithMessage("الدرجة النهائية يجب أن تكون أكبر من صفر");

        RuleFor(x => x.PassMarks)
            .LessThanOrEqualTo(x => x.MaxMarks).WithMessage("درجة النجاح لا يمكن أن تتجاوز الدرجة النهائية");
    }
}
