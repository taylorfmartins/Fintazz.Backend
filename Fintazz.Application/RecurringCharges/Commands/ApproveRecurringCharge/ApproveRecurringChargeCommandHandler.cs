namespace Fintazz.Application.RecurringCharges.Commands.ApproveRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;
using Fintazz.Application.Transactions.Commands.AddTransaction;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;
using MediatR;

public class ApproveRecurringChargeCommandHandler : ICommandHandler<ApproveRecurringChargeCommand, Guid>
{
    private readonly IRecurringChargeRepository _recurringChargeRepository;
    private readonly ISender _sender;

    public ApproveRecurringChargeCommandHandler(
        IRecurringChargeRepository recurringChargeRepository,
        ISender sender)
    {
        _recurringChargeRepository = recurringChargeRepository;
        _sender = sender;
    }

    public async Task<Result<Guid>> Handle(ApproveRecurringChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = await _recurringChargeRepository.GetByIdAsync(request.RecurringChargeId, cancellationToken);

        if (charge is null)
            return Result<Guid>.Failure(new Error("RecurringCharge.NotFound", "Cobrança recorrente não encontrada."));

        if (!charge.IsActive)
            return Result<Guid>.Failure(new Error("RecurringCharge.Inactive", "Não é possível aprovar uma cobrança recorrente inativa."));

        var amount = request.Amount ?? charge.Amount;

        if (charge.BankAccountId.HasValue)
        {
            var command = new AddTransactionCommand(
                charge.HouseHoldId,
                charge.BankAccountId.Value,
                charge.Description,
                amount,
                DateTime.UtcNow,
                TransactionType.Expense,
                IsPaid: true,
                charge.CategoryId);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            return Result<Guid>.Success(result.Value);
        }

        if (charge.CreditCardId.HasValue)
        {
            var command = new AddCreditCardPurchaseCommand(
                charge.CreditCardId.Value,
                charge.Description,
                amount,
                DateTime.UtcNow,
                TotalInstallments: 1);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            return Result<Guid>.Success(result.Value);
        }

        return Result<Guid>.Failure(new Error("RecurringCharge.Invalid", "A cobrança recorrente não possui meio de pagamento configurado."));
    }
}
