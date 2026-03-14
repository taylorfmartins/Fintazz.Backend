namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class ProductRepository : MongoRepository<Product>, IProductRepository
{
    public ProductRepository(MongoContext context) : base(context, "Products")
    {
    }

    public async Task<Product?> GetByEanAsync(string ean, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Ean, ean);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}
