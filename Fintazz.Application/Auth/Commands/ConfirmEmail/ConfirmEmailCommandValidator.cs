namespace Fintazz.Application.Auth.Commands.ConfirmEmail;

using FluentValidation;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token é obrigatório.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("E-mail inválido.");
    }
}
