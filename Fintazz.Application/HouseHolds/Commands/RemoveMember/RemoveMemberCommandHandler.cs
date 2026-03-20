namespace Fintazz.Application.HouseHolds.Commands.RemoveMember;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class RemoveMemberCommandHandler : ICommandHandler<RemoveMemberCommand>
{
    private readonly IHouseHoldRepository _houseHoldRepository;

    public RemoveMemberCommandHandler(IHouseHoldRepository houseHoldRepository)
    {
        _houseHoldRepository = houseHoldRepository;
    }

    public async Task<Result> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        if (!houseHold.IsAdmin(request.RequestingUserId))
            return Result.Failure(new Error("HouseHold.Forbidden", "Apenas o Administrador pode remover membros."));

        if (request.UserIdToRemove == houseHold.AdminUserId)
            return Result.Failure(new Error("HouseHold.CannotRemoveAdmin", "O Administrador não pode ser removido do grupo."));

        if (!houseHold.IsMember(request.UserIdToRemove))
            return Result.Failure(new Error("HouseHold.MemberNotFound", "O usuário não é membro deste grupo."));

        houseHold.RemoveMember(request.UserIdToRemove);
        await _houseHoldRepository.UpdateAsync(houseHold, cancellationToken);

        return Result.Success();
    }
}
