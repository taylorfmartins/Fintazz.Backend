namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IRecurringChargeRepository : IBaseRepository<RecurringCharge>
{
    Task<IEnumerable<RecurringCharge>> GetActiveByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecurringCharge>> GetActiveByBillingDayAsync(int day, CancellationToken cancellationToken = default);
}
