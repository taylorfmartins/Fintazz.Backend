namespace Fintazz.Application.Transactions.Commands.AddTransaction;

using FluentValidation;

public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    public AddTransactionCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("O ID da residência (HouseHold) é obrigatório.");

        RuleFor(x => x.BankAccountId)
            .NotEmpty().WithMessage("O ID da conta bancária/saldo é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(150).WithMessage("A descrição deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data da transação é obrigatória.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("O tipo de transação fornecido não é válido.");
    }
}
