namespace Fintazz.Application.CreditCards.Commands.UpdateCreditCard;

using FluentValidation;

public class UpdateCreditCardCommandValidator : AbstractValidator<UpdateCreditCardCommand>
{
    public UpdateCreditCardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cartão é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.TotalLimit)
            .GreaterThan(0).WithMessage("O limite total deve ser maior que zero.");

        RuleFor(x => x.ClosingDay)
            .InclusiveBetween(1, 28).WithMessage("O dia de fechamento deve ser entre 1 e 28.");

        RuleFor(x => x.DueDay)
            .InclusiveBetween(1, 28).WithMessage("O dia de vencimento deve ser entre 1 e 28.");
    }
}
