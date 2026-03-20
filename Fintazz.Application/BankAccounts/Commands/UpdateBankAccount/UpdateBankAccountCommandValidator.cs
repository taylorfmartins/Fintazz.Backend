namespace Fintazz.Application.BankAccounts.Commands.UpdateBankAccount;

using FluentValidation;

public class UpdateBankAccountCommandValidator : AbstractValidator<UpdateBankAccountCommand>
{
    public UpdateBankAccountCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.Name is not null || x.InitialBalance is not null || x.CurrentBalance is not null)
            .WithMessage("Informe pelo menos um campo para atualizar: Nome, Saldo Inicial ou Saldo Atual.");

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da conta não pode ser vazio.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
        });
    }
}
