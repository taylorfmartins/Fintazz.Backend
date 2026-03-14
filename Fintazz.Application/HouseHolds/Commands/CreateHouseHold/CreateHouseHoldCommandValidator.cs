namespace Fintazz.Application.HouseHolds.Commands.CreateHouseHold;

using FluentValidation;

public class CreateHouseHoldCommandValidator : AbstractValidator<CreateHouseHoldCommand>
{
    public CreateHouseHoldCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da Casa/Residência é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
    }
}
