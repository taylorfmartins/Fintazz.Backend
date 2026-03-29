namespace Fintazz.Application.CreditCardPurchases.Commands.MarkInstallmentAsPaid;

using FluentValidation;

public class MarkInstallmentAsPaidCommandValidator : AbstractValidator<MarkInstallmentAsPaidCommand>
{
    public MarkInstallmentAsPaidCommandValidator()
    {
        RuleFor(x => x.PurchaseId).NotEmpty().WithMessage("ID da compra é obrigatório.");
        RuleFor(x => x.InstallmentId).NotEmpty().WithMessage("ID da parcela é obrigatório.");
    }
}
