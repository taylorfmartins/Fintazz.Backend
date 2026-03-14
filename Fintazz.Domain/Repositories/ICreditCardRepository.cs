namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface ICreditCardRepository : IBaseRepository<CreditCard>
{
    Task<IEnumerable<CreditCard>> GetCardsByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
}
