namespace Fintazz.Application.CreditCards.Commands.CreateCreditCard;

using FluentValidation;

public class CreateCreditCardCommandValidator : AbstractValidator<CreateCreditCardCommand>
{
    public CreateCreditCardCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("O cartão deve pertencer a uma conta (HouseHold).");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cartão é obrigatório.")
            .MaximumLength(50).WithMessage("Permitido no máximo 50 caracteres.");

        RuleFor(x => x.TotalLimit)
            .GreaterThan(0).WithMessage("O limite total do cartão deve ser maior que zero.");

        RuleFor(x => x.ClosingDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de fechamento deve ser um dia válido (1 a 31).");

        RuleFor(x => x.DueDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de vencimento deve ser um dia válido (1 a 31).");
    }
}
