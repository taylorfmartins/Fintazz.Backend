namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class RecurringChargeRepository : MongoRepository<RecurringCharge>, IRecurringChargeRepository
{
    public RecurringChargeRepository(MongoContext context) : base(context, "RecurringCharges")
    {
    }

    public async Task<IEnumerable<RecurringCharge>> GetActiveByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(x => x.HouseHoldId == houseHoldId && x.IsActive)
                                .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RecurringCharge>> GetActiveByBillingDayAsync(int day, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(x => x.BillingDay == day && x.IsActive)
                                .ToListAsync(cancellationToken);
    }
}
