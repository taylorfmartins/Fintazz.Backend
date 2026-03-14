namespace Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;

using FluentValidation;

public class AddCreditCardPurchaseCommandValidator : AbstractValidator<AddCreditCardPurchaseCommand>
{
    public AddCreditCardPurchaseCommandValidator()
    {
        RuleFor(x => x.CreditCardId)
            .NotEmpty().WithMessage("O cartão de crédito deve ser informado.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição da compra não pode ficar vazia.")
            .MaximumLength(150).WithMessage("A descrição deve ter no máximo 150 caracteres.");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("O valor da compra deve ser superior a zero.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("A data da compra é obrigatória.");

        RuleFor(x => x.TotalInstallments)
            .GreaterThan(0).WithMessage("Deve haver ao menos 1 parcela.");
    }
}
