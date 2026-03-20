namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class HouseHoldInviteRepository : MongoRepository<HouseHoldInvite>, IHouseHoldInviteRepository
{
    public HouseHoldInviteRepository(MongoContext context) : base(context, "HouseHoldInvites") { }

    public async Task<HouseHoldInvite?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HouseHoldInvite>.Filter.Eq(x => x.Token, token);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<HouseHoldInvite?> GetPendingByEmailAndHouseHoldAsync(string email, Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HouseHoldInvite>.Filter.And(
            Builders<HouseHoldInvite>.Filter.Eq(x => x.InviteeEmail, email),
            Builders<HouseHoldInvite>.Filter.Eq(x => x.HouseHoldId, houseHoldId),
            Builders<HouseHoldInvite>.Filter.Eq(x => x.AcceptedAt, null));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteManyByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HouseHoldInvite>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }
}
