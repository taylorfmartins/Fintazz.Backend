namespace Fintazz.Application.Admin.Queries.GetUsers;

using Fintazz.Application.Abstractions.Messaging;

public record GetUsersQuery : IQuery<IEnumerable<UserSummaryResponse>>;

public record UserSummaryResponse(Guid Id, string FullName, string NickName, string Email, bool IsAdmin);
