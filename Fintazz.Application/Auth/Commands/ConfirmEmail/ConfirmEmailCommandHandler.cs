namespace Fintazz.Application.Auth.Commands.ConfirmEmail;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.IsEmailConfirmationTokenValid(request.Token))
            return Result.Failure(new Error("Auth.InvalidOrExpiredToken", "Token inválido ou expirado."));

        if (user.IsEmailConfirmed)
            return Result.Failure(new Error("Auth.EmailAlreadyConfirmed", "E-mail já confirmado."));

        user.ConfirmEmail();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
