namespace Fintazz.Application.Transactions.Commands.AddTransaction;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record AddTransactionCommand(
    Guid HouseHoldId,
    Guid BankAccountId,
    string Description,
    decimal Amount,
    DateTime Date,
    TransactionType Type,
    bool IsPaid,
    string? Category) : ICommand<Guid>;
