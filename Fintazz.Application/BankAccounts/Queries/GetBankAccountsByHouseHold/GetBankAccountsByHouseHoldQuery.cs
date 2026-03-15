namespace Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record GetBankAccountsByHouseHoldQuery(Guid HouseHoldId) : IQuery<IEnumerable<BankAccount>>;
