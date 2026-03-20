namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IEnumerable<Category>> GetByHouseHoldAsync(Guid houseHoldId, CancellationToken cancellationToken = default);
    Task<Category?> GetByNameAndTypeAsync(Guid houseHoldId, string name, CategoryType type, CancellationToken cancellationToken = default);
    Task<bool> IsInUseAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
