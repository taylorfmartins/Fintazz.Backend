namespace Fintazz.Application.Admin.Queries.GetUsers;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUsersQuery, IEnumerable<UserSummaryResponse>>
{
    public async Task<Result<IEnumerable<UserSummaryResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        var response = users.Select(u => new UserSummaryResponse(u.Id, u.FullName, u.NickName, u.Email, u.IsAdmin));
        return Result<IEnumerable<UserSummaryResponse>>.Success(response);
    }
}
