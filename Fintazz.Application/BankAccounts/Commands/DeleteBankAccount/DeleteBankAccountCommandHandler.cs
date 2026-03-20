namespace Fintazz.Application.BankAccounts.Commands.DeleteBankAccount;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteBankAccountCommandHandler : ICommandHandler<DeleteBankAccountCommand>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IRecurringChargeRepository _recurringChargeRepository;

    public DeleteBankAccountCommandHandler(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IRecurringChargeRepository recurringChargeRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _recurringChargeRepository = recurringChargeRepository;
    }

    public async Task<Result> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (account is null)
            return Result.Failure(new Error("BankAccount.NotFound", "Conta bancária não encontrada."));

        await _transactionRepository.DeleteManyByBankAccountAsync(request.BankAccountId, cancellationToken);
        await _recurringChargeRepository.DeactivateByBankAccountIdAsync(request.BankAccountId, cancellationToken);
        await _bankAccountRepository.DeleteAsync(request.BankAccountId, cancellationToken);

        return Result.Success();
    }
}
