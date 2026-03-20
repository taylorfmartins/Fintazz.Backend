namespace Fintazz.Application.HouseHolds.Commands.UpdateHouseHold;

using FluentValidation;

public class UpdateHouseHoldCommandValidator : AbstractValidator<UpdateHouseHoldCommand>
{
    public UpdateHouseHoldCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("O ID do grupo é obrigatório.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da Casa/Residência é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.RequestingUserId)
            .NotEmpty().WithMessage("O ID do usuário solicitante é obrigatório.");
    }
}
