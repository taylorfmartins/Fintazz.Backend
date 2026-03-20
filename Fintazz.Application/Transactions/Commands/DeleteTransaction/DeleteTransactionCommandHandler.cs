namespace Fintazz.Application.Transactions.Commands.DeleteTransaction;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Services;
using Fintazz.Domain.Shared;

public class DeleteTransactionCommandHandler : ICommandHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public DeleteTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository)
    {
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
            return Result.Failure(new Error("Transaction.NotFound", "Transação não encontrada."));

        // Se a transação estava paga, o saldo precisa ser revertido
        if (transaction.IsPaid)
        {
            var account = await _bankAccountRepository.GetByIdAsync(transaction.BankAccountId, cancellationToken);

            if (account is not null)
            {
                if (transaction is IncomeTransaction)
                    account.RemoveTransaction(transaction.Amount);
                else if (transaction is ExpenseTransaction)
                    account.AddTransaction(transaction.Amount);

                await _bankAccountRepository.UpdateAsync(account, cancellationToken);
            }
        }

        await _transactionRepository.DeleteAsync(request.TransactionId, cancellationToken);

        return Result.Success();
    }
}
