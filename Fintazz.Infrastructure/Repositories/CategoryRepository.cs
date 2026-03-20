namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class CategoryRepository : MongoRepository<Category>, ICategoryRepository
{
    private readonly MongoContext _context;

    public CategoryRepository(MongoContext context) : base(context, "Categories")
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.HouseHoldId, houseHoldId);
        return await Collection.Find(filter).SortBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByNameAndTypeAsync(Guid houseHoldId, string name, CategoryType type, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.And(
            Builders<Category>.Filter.Eq(x => x.HouseHoldId, houseHoldId),
            Builders<Category>.Filter.Eq(x => x.Name, name),
            Builders<Category>.Filter.Eq(x => x.Type, type));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsInUseAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var transactionFilter = Builders<Transaction>.Filter.Eq(x => x.CategoryId, categoryId);
        var transactionCount = await _context.GetCollection<Transaction>("Transactions")
            .CountDocumentsAsync(transactionFilter, cancellationToken: cancellationToken);

        if (transactionCount > 0)
            return true;

        var recurringFilter = Builders<RecurringCharge>.Filter.Eq(x => x.CategoryId, categoryId);
        var recurringCount = await _context.GetCollection<RecurringCharge>("RecurringCharges")
            .CountDocumentsAsync(recurringFilter, cancellationToken: cancellationToken);

        return recurringCount > 0;
    }
}
