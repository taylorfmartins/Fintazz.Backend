namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetTransactionsByHouseHoldAsync(Guid houseHoldId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
    Task DeleteManyByBankAccountAsync(Guid bankAccountId, CancellationToken cancellationToken = default);
}
