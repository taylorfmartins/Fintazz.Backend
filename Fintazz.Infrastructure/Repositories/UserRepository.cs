namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class UserRepository : MongoRepository<User>, IUserRepository
{
    public UserRepository(MongoContext context) : base(context, "Users") { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.RefreshToken, refreshToken);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(Builders<User>.Filter.Empty).SortBy(u => u.FullName).ToListAsync(cancellationToken);
    }
}
