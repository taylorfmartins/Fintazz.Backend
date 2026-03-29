namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IEnumerable<Category>> GetByHouseHoldAndUserAsync(Guid houseHoldId, Guid userId, CancellationToken cancellationToken = default);
    Task<Category?> GetByNameAndTypeAsync(Guid houseHoldId, string name, CategoryType type, CancellationToken cancellationToken = default);
    Task<bool> IsInUseAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> HasSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task SeedSystemCategoriesAsync(CancellationToken cancellationToken = default);
}
