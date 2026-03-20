namespace Fintazz.Application.Transactions.Commands.MarkTransactionAsPaid;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Services;
using Fintazz.Domain.Shared;

public class MarkTransactionAsPaidCommandHandler : ICommandHandler<MarkTransactionAsPaidCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public MarkTransactionAsPaidCommandHandler(
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository)
    {
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result> Handle(MarkTransactionAsPaidCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
            return Result.Failure(new Error("Transaction.NotFound", "Transação não encontrada."));

        if (transaction.IsPaid)
            return Result.Failure(new Error("Transaction.AlreadyPaid", "Esta transação já está marcada como paga."));

        var account = await _bankAccountRepository.GetByIdAsync(transaction.BankAccountId, cancellationToken);

        if (account is null)
            return Result.Failure(new Error("BankAccount.NotFound", "Conta bancária da transação não encontrada."));

        transaction.MarkAsPaid();
        BalanceEngine.ProcessTransaction(account, transaction);

        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
        await _bankAccountRepository.UpdateAsync(account, cancellationToken);

        return Result.Success();
    }
}
