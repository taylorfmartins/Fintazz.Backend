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

    public async Task<(IEnumerable<Transaction> Items, long TotalCount)> GetTransactionsByHouseHoldPagedAsync(Guid houseHoldId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.Eq(x => x.HouseHoldId, houseHoldId);

        if (startDate.HasValue)
            filter &= builder.Gte(x => x.Date, startDate.Value);

        if (endDate.HasValue)
            filter &= builder.Lte(x => x.Date, endDate.Value);

        var skip = (page - 1) * pageSize;

        var countTask = Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var itemsTask = Collection.Find(filter).SortByDescending(x => x.Date).Skip(skip).Limit(pageSize).ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);

        return (await itemsTask, await countTask);
    }

    public async Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Transaction>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }

    public async Task DeleteManyByBankAccountAsync(Guid bankAccountId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Transaction>.Filter.Eq(x => x.BankAccountId, bankAccountId);
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }
}
