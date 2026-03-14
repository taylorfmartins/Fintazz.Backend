namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IEstablishmentRepository : IBaseRepository<Establishment>
{
    Task<Establishment?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);
}
