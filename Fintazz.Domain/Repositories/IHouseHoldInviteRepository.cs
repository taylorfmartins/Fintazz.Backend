namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IHouseHoldInviteRepository : IBaseRepository<HouseHoldInvite>
{
    Task<HouseHoldInvite?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<HouseHoldInvite?> GetPendingByEmailAndHouseHoldAsync(string email, Guid houseHoldId, CancellationToken cancellationToken = default);
    Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
}
