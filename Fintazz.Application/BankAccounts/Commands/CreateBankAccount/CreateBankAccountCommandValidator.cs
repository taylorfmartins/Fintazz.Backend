namespace Fintazz.Application.BankAccounts.Commands.CreateBankAccount;

using FluentValidation;

public class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountCommandValidator()
    {
        RuleFor(x => x.HouseHoldId)
            .NotEmpty().WithMessage("A identificação da Casa (HouseHoldId) deve ser preenchida.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da conta é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da conta deve ter no máximo 100 caracteres.");
    }
}
