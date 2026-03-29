namespace Fintazz.Application.Users.Commands.ChangePassword;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;

    public ChangePasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return Result.Failure(new Error("User.InvalidPassword", "Senha atual incorreta."));

        var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(newHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
