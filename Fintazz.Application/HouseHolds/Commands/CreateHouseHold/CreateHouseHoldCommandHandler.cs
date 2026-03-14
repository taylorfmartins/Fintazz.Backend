namespace Fintazz.Application.HouseHolds.Commands.CreateHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class CreateHouseHoldCommandHandler : ICommandHandler<CreateHouseHoldCommand, Guid>
{
    private readonly IHouseHoldRepository _houseHoldRepository;

    public CreateHouseHoldCommandHandler(IHouseHoldRepository houseHoldRepository)
    {
        _houseHoldRepository = houseHoldRepository;
    }

    public async Task<Result<Guid>> Handle(CreateHouseHoldCommand request, CancellationToken cancellationToken)
    {
        var houseHold = new HouseHold(Guid.NewGuid(), request.Name);

        await _houseHoldRepository.AddAsync(houseHold, cancellationToken);

        return Result<Guid>.Success(houseHold.Id);
    }
}
