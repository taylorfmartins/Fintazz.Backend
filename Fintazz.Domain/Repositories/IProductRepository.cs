namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetByEanAsync(string ean, CancellationToken cancellationToken = default);
}
