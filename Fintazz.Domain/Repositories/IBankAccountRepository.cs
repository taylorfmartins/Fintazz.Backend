namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IBankAccountRepository : IBaseRepository<BankAccount>
{
    Task<IEnumerable<BankAccount>> GetBankAccountsByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
    Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
}
