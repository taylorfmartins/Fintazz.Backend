namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IHouseHoldRepository : IBaseRepository<HouseHold>
{
    Task<IEnumerable<HouseHold>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
