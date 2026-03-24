namespace Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Shared;

public record GetTransactionsByHouseHoldQuery(
    Guid HouseHoldId,
    int Page = 1,
    int PageSize = 20,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IQuery<PagedResult<TransactionResponse>>;
