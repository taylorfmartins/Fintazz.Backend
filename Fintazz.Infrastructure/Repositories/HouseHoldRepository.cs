namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;

public class HouseHoldRepository : MongoRepository<HouseHold>, IHouseHoldRepository
{
    public HouseHoldRepository(MongoContext context) : base(context, "HouseHolds")
    {
    }
}
