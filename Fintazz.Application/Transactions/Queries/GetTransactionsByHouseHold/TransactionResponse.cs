namespace Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;

using Fintazz.Domain.Entities;

/// <summary>
/// Dados de uma transação financeira com nomes dos vínculos resolvidos.
/// </summary>
/// <param name="Id">ID da transação.</param>
/// <param name="Description">Descrição da transação.</param>
/// <param name="Amount">Valor da transação.</param>
/// <param name="Date">Data da transação.</param>
/// <param name="Type">Tipo da transação (Income, Expense, Subscription).</param>
/// <param name="IsPaid">Indica se a transação já foi efetivada/paga.</param>
/// <param name="BankAccountId">ID da conta bancária vinculada.</param>
/// <param name="BankAccountName">Nome da conta bancária vinculada.</param>
/// <param name="CategoryId">ID da categoria vinculada.</param>
/// <param name="CategoryName">Nome da categoria vinculada.</param>
/// <param name="AutoRenew">Para transações do tipo Subscription: indica renovação automática.</param>
public record TransactionResponse(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    TransactionType Type,
    bool IsPaid,
    Guid BankAccountId,
    string BankAccountName,
    Guid? CategoryId,
    string? CategoryName,
    bool? AutoRenew);
