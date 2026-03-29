namespace Fintazz.Application.Auth.Commands.ResetPassword;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.IsPasswordResetTokenValid(request.Token))
            return Result.Failure(new Error("Auth.InvalidOrExpiredToken", "Token inválido ou expirado."));

        var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(newHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
