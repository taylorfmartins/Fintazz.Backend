namespace Fintazz.Application.Users.Queries.GetUserProfile;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result<UserProfileResponse>.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        return Result<UserProfileResponse>.Success(new UserProfileResponse(
            Id:        user.Id,
            FullName:  user.FullName,
            NickName:  user.NickName,
            Email:     user.Email,
            BirthDate: user.BirthDate));
    }
}
