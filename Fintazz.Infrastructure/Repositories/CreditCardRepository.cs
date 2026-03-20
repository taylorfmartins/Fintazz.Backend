namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class CreditCardRepository : MongoRepository<CreditCard>, ICreditCardRepository
{
    public CreditCardRepository(MongoContext context) : base(context, "CreditCards")
    {
    }

    public async Task<IEnumerable<CreditCard>> GetCardsByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CreditCard>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CreditCard>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }
}
