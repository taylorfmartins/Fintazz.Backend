namespace Fintazz.Application.Auth.Commands.ResendEmailConfirmation;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Abstractions.Services;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ResendEmailConfirmationCommandHandler : ICommandHandler<ResendEmailConfirmationCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailQueue _emailQueue;

    public ResendEmailConfirmationCommandHandler(IUserRepository userRepository, IEmailQueue emailQueue)
    {
        _userRepository = userRepository;
        _emailQueue = emailQueue;
    }

    public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Retorna sucesso mesmo se e-mail não existir para não expor dados
        if (user is null || user.IsEmailConfirmed)
            return Result.Success();

        var token = Guid.NewGuid().ToString("N");
        user.SetEmailConfirmationToken(token, DateTime.UtcNow.AddHours(24));

        await _userRepository.UpdateAsync(user, cancellationToken);
        _emailQueue.EnqueueEmailConfirmation(user.Email, user.NickName, token);

        return Result.Success();
    }
}
