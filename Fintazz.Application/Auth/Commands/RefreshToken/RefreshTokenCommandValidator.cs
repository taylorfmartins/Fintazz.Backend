namespace Fintazz.Application.Auth.Commands.RefreshToken;

using FluentValidation;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O RefreshToken é obrigatório.");
    }
}
