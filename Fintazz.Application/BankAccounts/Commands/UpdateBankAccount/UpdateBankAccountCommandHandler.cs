namespace Fintazz.Application.BankAccounts.Commands.UpdateBankAccount;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateBankAccountCommandHandler : ICommandHandler<UpdateBankAccountCommand>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public UpdateBankAccountCommandHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (account is null)
            return Result.Failure(new Error("BankAccount.NotFound", "Conta bancária não encontrada."));

        if (request.Name is not null)
            account.UpdateName(request.Name);

        if (request.InitialBalance.HasValue)
            account.UpdateInitialBalance(request.InitialBalance.Value);

        if (request.CurrentBalance.HasValue)
            account.SetCurrentBalance(request.CurrentBalance.Value);

        await _bankAccountRepository.UpdateAsync(account, cancellationToken);

        return Result.Success();
    }
}
