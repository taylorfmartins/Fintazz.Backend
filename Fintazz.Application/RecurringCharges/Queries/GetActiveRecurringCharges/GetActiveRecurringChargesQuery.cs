namespace Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;

using Fintazz.Application.Abstractions.Messaging;

public record GetActiveRecurringChargesQuery(Guid HouseHoldId) : IQuery<IEnumerable<RecurringChargeResponse>>;
