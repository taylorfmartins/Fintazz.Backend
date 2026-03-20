namespace Fintazz.Application.HouseHolds.Queries.GetHouseHoldMembers;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Lista os membros de um grupo familiar.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
public record GetHouseHoldMembersQuery(Guid HouseHoldId) : IQuery<IEnumerable<HouseHoldMemberResponse>>;
