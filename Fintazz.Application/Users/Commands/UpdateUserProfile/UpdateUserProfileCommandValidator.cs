namespace Fintazz.Application.Users.Commands.UpdateUserProfile;

using FluentValidation;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Nome completo é obrigatório.")
            .MaximumLength(150).WithMessage("Nome completo deve ter no máximo 150 caracteres.");

        RuleFor(x => x.NickName).NotEmpty().WithMessage("Apelido é obrigatório.")
            .MaximumLength(50).WithMessage("Apelido deve ter no máximo 50 caracteres.");

        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Data de nascimento deve ser no passado.");
    }
}
