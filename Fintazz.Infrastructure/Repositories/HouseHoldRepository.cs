namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class HouseHoldRepository : MongoRepository<HouseHold>, IHouseHoldRepository
{
    public HouseHoldRepository(MongoContext context) : base(context, "HouseHolds") { }

    public async Task<IEnumerable<HouseHold>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HouseHold>.Filter.AnyEq(x => x.MemberIds, userId);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }
}
