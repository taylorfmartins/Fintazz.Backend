namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
}
