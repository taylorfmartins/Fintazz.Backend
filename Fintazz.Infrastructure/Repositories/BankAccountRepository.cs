namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class BankAccountRepository : MongoRepository<BankAccount>, IBankAccountRepository
{
    public BankAccountRepository(MongoContext context) : base(context, "BankAccounts")
    {
    }

    public async Task<IEnumerable<BankAccount>> GetBankAccountsByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BankAccount>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }
}
