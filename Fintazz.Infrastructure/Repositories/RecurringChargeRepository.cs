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

    public async Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RecurringCharge>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }

    public async Task DeactivateByCreditCardIdAsync(Guid creditCardId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RecurringCharge>.Filter.Eq(x => x.CreditCardId, creditCardId);
        var update = Builders<RecurringCharge>.Update.Set(x => x.IsActive, false);
        await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeactivateByBankAccountIdAsync(Guid bankAccountId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RecurringCharge>.Filter.Eq(x => x.BankAccountId, bankAccountId);
        var update = Builders<RecurringCharge>.Update.Set(x => x.IsActive, false);
        await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }
}
