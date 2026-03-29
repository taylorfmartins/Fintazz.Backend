namespace Fintazz.Application.Users.Commands.UpdateUserProfile;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        user.UpdateProfile(request.FullName, request.NickName, request.BirthDate);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
