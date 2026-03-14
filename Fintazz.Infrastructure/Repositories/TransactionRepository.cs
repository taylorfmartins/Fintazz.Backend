namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class TransactionRepository : MongoRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(MongoContext context) : base(context, "Transactions")
    {
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByHouseHoldAsync(Guid houseHoldId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.Eq(x => x.HouseHoldId, houseHoldId);

        if (startDate.HasValue)
            filter &= builder.Gte(x => x.Date, startDate.Value);

        if (endDate.HasValue)
            filter &= builder.Lte(x => x.Date, endDate.Value);

        return await Collection.Find(filter).SortByDescending(x => x.Date).ToListAsync(cancellationToken);
    }
}
