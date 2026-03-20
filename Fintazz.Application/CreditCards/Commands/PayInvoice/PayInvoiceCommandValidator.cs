namespace Fintazz.Application.CreditCards.Commands.PayInvoice;

using FluentValidation;

public class PayInvoiceCommandValidator : AbstractValidator<PayInvoiceCommand>
{
    public PayInvoiceCommandValidator()
    {
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("O mês deve ser entre 1 e 12.");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("O ano informado é inválido.");
    }
}
