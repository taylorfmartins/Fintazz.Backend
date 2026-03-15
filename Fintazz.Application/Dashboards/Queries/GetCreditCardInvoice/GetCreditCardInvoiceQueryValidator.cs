namespace Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;

using FluentValidation;

public class GetCreditCardInvoiceQueryValidator : AbstractValidator<GetCreditCardInvoiceQuery>
{
    public GetCreditCardInvoiceQueryValidator()
    {
        RuleFor(v => v.CreditCardId)
            .NotEmpty().WithMessage("O identificador do Cartão de Crédito é obrigatório.");

        RuleFor(v => v.Month)
            .InclusiveBetween(1, 12).WithMessage("O mês deve estar entre 1 e 12.");

        RuleFor(v => v.Year)
            .GreaterThan(2000).WithMessage("O ano deve ser maior que 2000.");
    }
}
