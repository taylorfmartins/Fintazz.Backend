namespace Fintazz.Application.BankAccounts.Commands.CreateBankAccount;

using Fintazz.Application.Abstractions.Messaging;

public record CreateBankAccountCommand(
    Guid HouseHoldId,
    string Name,
    decimal InitialBalance) : ICommand<Guid>;
