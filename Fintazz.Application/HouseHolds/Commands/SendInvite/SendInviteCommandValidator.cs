namespace Fintazz.Application.HouseHolds.Commands.SendInvite;

using FluentValidation;

public class SendInviteCommandValidator : AbstractValidator<SendInviteCommand>
{
    public SendInviteCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("O ID do grupo é obrigatório.");

        RuleFor(x => x.InviteeEmail)
            .NotEmpty().WithMessage("O e-mail do convidado é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.RequestingUserId)
            .NotEmpty().WithMessage("O ID do usuário solicitante é obrigatório.");
    }
}
