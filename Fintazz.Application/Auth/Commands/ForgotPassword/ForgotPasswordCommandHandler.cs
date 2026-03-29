namespace Fintazz.Application.Auth.Commands.ForgotPassword;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Abstractions.Services;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailQueue _emailQueue;

    public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailQueue emailQueue)
    {
        _userRepository = userRepository;
        _emailQueue = emailQueue;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Retorna sucesso mesmo se o e-mail não existir para não expor quais e-mails estão cadastrados
        if (user is null)
            return Result.Success();

        var token = Guid.NewGuid().ToString("N");
        user.SetPasswordResetToken(token, DateTime.UtcNow.AddHours(2));

        await _userRepository.UpdateAsync(user, cancellationToken);
        _emailQueue.EnqueuePasswordReset(user.Email, user.NickName, token);

        return Result.Success();
    }
}
