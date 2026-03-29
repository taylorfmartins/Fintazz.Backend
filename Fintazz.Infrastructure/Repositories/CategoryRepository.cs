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

    public async Task<IEnumerable<Category>> GetByHouseHoldAndUserAsync(Guid houseHoldId, Guid userId, CancellationToken cancellationToken = default)
    {
        // Retorna categorias de sistema (IsSystem = true) + categorias do grupo familiar
        var filter = Builders<Category>.Filter.Or(
            Builders<Category>.Filter.Eq(x => x.IsSystem, true),
            Builders<Category>.Filter.Eq(x => x.HouseHoldId, houseHoldId)
        );
        return await Collection.Find(filter).SortBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task SeedSystemCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var exists = await Collection.Find(Builders<Category>.Filter.Eq(x => x.IsSystem, true))
            .AnyAsync(cancellationToken);

        if (exists) return;

        var systemId = Guid.Empty;

        var categories = new[]
        {
            // Despesas
            new Category(Guid.NewGuid(), systemId, "Alimentação", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Moradia", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Transporte", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Saúde", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Educação", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Lazer", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Vestuário", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Assinaturas", CategoryType.Expense, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Outros", CategoryType.Expense, systemId, isSystem: true),
            // Receitas
            new Category(Guid.NewGuid(), systemId, "Salário", CategoryType.Income, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Freelance", CategoryType.Income, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Investimentos", CategoryType.Income, systemId, isSystem: true),
            new Category(Guid.NewGuid(), systemId, "Outros", CategoryType.Income, systemId, isSystem: true),
        };

        await Collection.InsertManyAsync(categories, cancellationToken: cancellationToken);
    }

    public async Task<Category?> GetByNameAndTypeAsync(Guid houseHoldId, string name, CategoryType type, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.And(
            Builders<Category>.Filter.Eq(x => x.HouseHoldId, houseHoldId),
            Builders<Category>.Filter.Eq(x => x.Name, name),
            Builders<Category>.Filter.Eq(x => x.Type, type));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HasSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.ParentCategoryId, categoryId);
        return await Collection.Find(filter).AnyAsync(cancellationToken);
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
