namespace Fintazz.Application.RecurringCharges.Commands.DeleteRecurringCharge;

using MediatR;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteRecurringChargeCommandHandler : IRequestHandler<DeleteRecurringChargeCommand, Result>
{
    private readonly IRecurringChargeRepository _repository;

    public DeleteRecurringChargeCommandHandler(IRecurringChargeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteRecurringChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (charge == null)
            return Result.Failure(new Error("RecurringCharge.NotFound", "Cobrança recorrente não encontrada."));

        charge.Deactivate();
        await _repository.UpdateAsync(charge, cancellationToken);

        return Result.Success();
    }
}
