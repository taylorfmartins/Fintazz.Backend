namespace Fintazz.Application.RecurringCharges.Commands.UpdateRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateRecurringChargeCommandHandler : ICommandHandler<UpdateRecurringChargeCommand>
{
    private readonly IRecurringChargeRepository _recurringChargeRepository;

    public UpdateRecurringChargeCommandHandler(IRecurringChargeRepository recurringChargeRepository)
    {
        _recurringChargeRepository = recurringChargeRepository;
    }

    public async Task<Result> Handle(UpdateRecurringChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = await _recurringChargeRepository.GetByIdAsync(request.RecurringChargeId, cancellationToken);

        if (charge is null)
            return Result.Failure(new Error("RecurringCharge.NotFound", "Cobrança recorrente não encontrada."));

        // Preserva campos imutáveis e atualiza apenas os permitidos
        charge.UpdateDetails(
            request.Description,
            request.Amount,
            charge.BillingDay,
            request.CategoryId,
            charge.IsVariableAmount,
            charge.AutoApprove);

        await _recurringChargeRepository.UpdateAsync(charge, cancellationToken);

        return Result.Success();
    }
}
