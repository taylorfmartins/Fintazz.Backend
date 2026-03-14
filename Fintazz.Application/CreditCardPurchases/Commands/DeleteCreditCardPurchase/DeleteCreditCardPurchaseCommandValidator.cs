namespace Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;

using FluentValidation;

public class DeleteCreditCardPurchaseCommandValidator : AbstractValidator<DeleteCreditCardPurchaseCommand>
{
    public DeleteCreditCardPurchaseCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty().WithMessage("O ID da compra deve ser informado para exclusão.");
    }
}
