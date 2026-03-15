namespace Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record GetTransactionsByHouseHoldQuery(
    Guid HouseHoldId,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IQuery<IEnumerable<Transaction>>;
