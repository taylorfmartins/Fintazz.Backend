namespace Fintazz.Application.RecurringCharges.Commands.UpdateRecurringCharge;

using FluentValidation;

public class UpdateRecurringChargeCommandValidator : AbstractValidator<UpdateRecurringChargeCommand>
{
    public UpdateRecurringChargeCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
    }
}
