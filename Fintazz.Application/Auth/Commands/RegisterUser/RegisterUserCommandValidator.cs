namespace Fintazz.Application.Auth.Commands.RegisterUser;

using FluentValidation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MaximumLength(150).WithMessage("O nome completo deve ter no máximo 150 caracteres.");

        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("O apelido é obrigatório.")
            .MaximumLength(50).WithMessage("O apelido deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("A data de nascimento deve ser anterior à data atual.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
    }
}
