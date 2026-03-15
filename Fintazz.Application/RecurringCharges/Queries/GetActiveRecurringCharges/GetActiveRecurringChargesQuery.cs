namespace Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;

using MediatR;
using Fintazz.Domain.Entities;

public record GetActiveRecurringChargesQuery(Guid HouseHoldId) : IRequest<IEnumerable<RecurringCharge>>;
