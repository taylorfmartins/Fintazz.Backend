namespace Fintazz.Application.Admin.Commands.GrantAdmin;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GrantAdminCommandHandler(IUserRepository userRepository) : ICommandHandler<GrantAdminCommand>
{
    public async Task<Result> Handle(GrantAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        user.MakeAdmin();
        await userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
