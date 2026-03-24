namespace Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;

public record GetBankAccountsByHouseHoldQuery(Guid HouseHoldId) : IQuery<IEnumerable<BankAccountResponse>>;
