namespace Fintazz.Application.Admin.Commands.RevokeAdmin;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class RevokeAdminCommandHandler(IUserRepository userRepository) : ICommandHandler<RevokeAdminCommand>
{
    public async Task<Result> Handle(RevokeAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        user.RevokeAdmin();
        await userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
