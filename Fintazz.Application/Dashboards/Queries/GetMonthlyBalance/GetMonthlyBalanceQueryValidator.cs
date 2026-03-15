namespace Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;

using FluentValidation;

public class GetMonthlyBalanceQueryValidator : AbstractValidator<GetMonthlyBalanceQuery>
{
    public GetMonthlyBalanceQueryValidator()
    {
        RuleFor(v => v.HouseHoldId)
            .NotEmpty().WithMessage("O identificador do HouseHold é obrigatório.");

        RuleFor(v => v.Month)
            .InclusiveBetween(1, 12).WithMessage("O mês deve estar entre 1 e 12.");

        RuleFor(v => v.Year)
            .GreaterThan(2000).WithMessage("O ano deve ser maior que 2000.");
    }
}
