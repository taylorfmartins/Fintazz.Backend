namespace Fintazz.Application.CreditCardPurchases.Commands.UpdateCreditCardPurchase;

using FluentValidation;

public class UpdateCreditCardPurchaseCommandValidator : AbstractValidator<UpdateCreditCardPurchaseCommand>
{
    public UpdateCreditCardPurchaseCommandValidator()
    {
        RuleFor(x => x.PurchaseId).NotEmpty().WithMessage("ID da compra é obrigatório.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(200).WithMessage("Descrição deve ter no máximo 200 caracteres.");
    }
}
