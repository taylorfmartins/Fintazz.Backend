namespace Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;

using MediatR;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;

public class GetActiveRecurringChargesQueryHandler : IRequestHandler<GetActiveRecurringChargesQuery, IEnumerable<RecurringCharge>>
{
    private readonly IRecurringChargeRepository _repository;

    public GetActiveRecurringChargesQueryHandler(IRecurringChargeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RecurringCharge>> Handle(GetActiveRecurringChargesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetActiveByHouseHoldAsync(request.HouseHoldId, cancellationToken);
    }
}
