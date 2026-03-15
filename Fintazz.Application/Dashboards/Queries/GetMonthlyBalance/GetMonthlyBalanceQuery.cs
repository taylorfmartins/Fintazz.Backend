namespace Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;

using Fintazz.Application.Abstractions.Messaging;

public record GetMonthlyBalanceQuery(Guid HouseHoldId, int Month, int Year) : IQuery<MonthlyBalanceResponse>;
