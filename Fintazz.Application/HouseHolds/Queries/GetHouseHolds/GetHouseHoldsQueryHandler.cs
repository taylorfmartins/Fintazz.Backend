namespace Fintazz.Application.HouseHolds.Queries.GetHouseHolds;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetHouseHoldsQueryHandler : IQueryHandler<GetHouseHoldsQuery, IEnumerable<HouseHold>>
{
    private readonly IHouseHoldRepository _houseHoldRepository;

    public GetHouseHoldsQueryHandler(IHouseHoldRepository houseHoldRepository)
    {
        _houseHoldRepository = houseHoldRepository;
    }

    public async Task<Result<IEnumerable<HouseHold>>> Handle(GetHouseHoldsQuery request, CancellationToken cancellationToken)
    {
        var result = await _houseHoldRepository.GetAllAsync(cancellationToken);
        
        return Result<IEnumerable<HouseHold>>.Success(result);
    }
}
