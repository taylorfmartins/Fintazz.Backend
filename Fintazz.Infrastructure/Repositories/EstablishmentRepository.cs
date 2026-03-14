namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class EstablishmentRepository : MongoRepository<Establishment>, IEstablishmentRepository
{
    public EstablishmentRepository(MongoContext context) : base(context, "Establishments")
    {
    }

    public async Task<Establishment?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Establishment>.Filter.Eq(x => x.Cnpj, cnpj);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}
