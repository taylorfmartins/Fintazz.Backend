namespace Fintazz.Application.RecurringCharges.Commands.ReactivateRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class ReactivateRecurringChargeCommandHandler : ICommandHandler<ReactivateRecurringChargeCommand>
{
    private readonly IRecurringChargeRepository _recurringChargeRepository;

    public ReactivateRecurringChargeCommandHandler(IRecurringChargeRepository recurringChargeRepository)
    {
        _recurringChargeRepository = recurringChargeRepository;
    }

    public async Task<Result> Handle(ReactivateRecurringChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = await _recurringChargeRepository.GetByIdAsync(request.RecurringChargeId, cancellationToken);

        if (charge is null)
            return Result.Failure(new Error("RecurringCharge.NotFound", "Cobrança recorrente não encontrada."));

        if (charge.IsActive)
            return Result.Failure(new Error("RecurringCharge.AlreadyActive", "A cobrança recorrente já está ativa."));

        charge.Activate();
        await _recurringChargeRepository.UpdateAsync(charge, cancellationToken);

        return Result.Success();
    }
}
