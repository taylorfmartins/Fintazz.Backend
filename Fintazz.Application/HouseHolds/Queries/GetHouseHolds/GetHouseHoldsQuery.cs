namespace Fintazz.Application.HouseHolds.Queries.GetHouseHolds;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record GetHouseHoldsQuery() : IQuery<IEnumerable<HouseHold>>;
