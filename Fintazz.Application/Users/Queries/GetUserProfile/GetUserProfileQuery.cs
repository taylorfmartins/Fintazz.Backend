namespace Fintazz.Application.Users.Queries.GetUserProfile;

using Fintazz.Application.Abstractions.Messaging;

public record GetUserProfileQuery(Guid UserId) : IQuery<UserProfileResponse>;
