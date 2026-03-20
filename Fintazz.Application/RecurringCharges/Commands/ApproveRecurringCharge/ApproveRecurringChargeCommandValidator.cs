namespace Fintazz.Application.RecurringCharges.Commands.ApproveRecurringCharge;

using FluentValidation;

public class ApproveRecurringChargeCommandValidator : AbstractValidator<ApproveRecurringChargeCommand>
{
    public ApproveRecurringChargeCommandValidator()
    {
        When(x => x.Amount.HasValue, () =>
        {
            RuleFor(x => x.Amount!.Value)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        });
    }
}
