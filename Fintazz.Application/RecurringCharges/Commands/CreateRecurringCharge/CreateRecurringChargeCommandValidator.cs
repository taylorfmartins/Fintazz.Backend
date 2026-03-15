namespace Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;

using FluentValidation;

public class CreateRecurringChargeCommandValidator : AbstractValidator<CreateRecurringChargeCommand>
{
    public CreateRecurringChargeCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("O identificador do HouseHold é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição da cobrança recorrente é obrigatória.")
            .MaximumLength(150).WithMessage("A descrição não pode ter mais que 150 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da cobrança recorrente deve ser maior que zero.");

        RuleFor(x => x.BillingDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de cobrança deve estar entre 1 e 31.");

        RuleFor(x => x).Custom((command, context) =>
        {
            if (command.BankAccountId.HasValue && command.CreditCardId.HasValue)
            {
                context.AddFailure("Apenas um método de pagamento pode ser escolhido: Ou Conta Bancária, Ou Cartão de Crédito.");
            }

            if (!command.BankAccountId.HasValue && !command.CreditCardId.HasValue)
            {
                context.AddFailure("É obrigatório escolher o método de pagamento (Conta Bancária ou Cartão de Crédito).");
            }
        });
    }
}
