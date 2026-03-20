namespace Fintazz.Application.HouseHolds.Commands.UpdateHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateHouseHoldCommandHandler : ICommandHandler<UpdateHouseHoldCommand>
{
    private readonly IHouseHoldRepository _houseHoldRepository;

    public UpdateHouseHoldCommandHandler(IHouseHoldRepository houseHoldRepository)
    {
        _houseHoldRepository = houseHoldRepository;
    }

    public async Task<Result> Handle(UpdateHouseHoldCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        if (!houseHold.IsAdmin(request.RequestingUserId))
            return Result.Failure(new Error("HouseHold.Forbidden", "Apenas o Administrador pode editar o grupo familiar."));

        houseHold.UpdateName(request.Name);
        await _houseHoldRepository.UpdateAsync(houseHold, cancellationToken);

        return Result.Success();
    }
}
