namespace Fintazz.Application.Transactions.Commands.AddTransaction;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Services;
using Fintazz.Domain.Shared;

public class AddTransactionCommandHandler : ICommandHandler<AddTransactionCommand, Guid>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public AddTransactionCommandHandler(
        IHouseHoldRepository houseHoldRepository, 
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result<Guid>> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold is null)
        {
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "A residência (HouseHold) informada não foi encontrada."));
        }

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);
        if (bankAccount is null)
        {
            return Result<Guid>.Failure(new Error("BankAccount.NotFound", "A conta bancária informada não foi encontrada."));
        }

        // Caso a transação seja de conta "diferente" da residência apontada, negamos também por segurança
        if (bankAccount.HouseHoldId != request.HouseHoldId)
        {
            return Result<Guid>.Failure(new Error("BankAccount.Invalid", "A conta bancária não pertence à residência informada."));
        }

        Transaction transaction = request.Type switch
        {
            TransactionType.Income => new IncomeTransaction(Guid.NewGuid(), request.HouseHoldId, request.BankAccountId, request.Description, request.Amount, request.Date, request.IsPaid, request.Category),
            TransactionType.Expense => new ExpenseTransaction(Guid.NewGuid(), request.HouseHoldId, request.BankAccountId, request.Description, request.Amount, request.Date, request.IsPaid, request.Category),
            TransactionType.Subscription => throw new NotImplementedException("Assinaturas possuem um fluxo separado."),
            _ => throw new ArgumentOutOfRangeException()
        };

        // Regra de Negócio principal delegada para o Domain Service centralizado de Saldos
        BalanceEngine.ProcessTransaction(bankAccount, transaction);

        if (request.IsPaid)
        {
            await _bankAccountRepository.UpdateAsync(bankAccount, cancellationToken);
        }

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return Result<Guid>.Success(transaction.Id);
    }
}
