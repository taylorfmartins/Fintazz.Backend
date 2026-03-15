namespace Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;

using MediatR;
using Fintazz.Domain.Shared;

public record CreateRecurringChargeCommand(
    Guid HouseHoldId,
    string Description,
    decimal Amount,
    int BillingDay,
    string? Category,
    Guid? BankAccountId,
    Guid? CreditCardId,
    bool IsVariableAmount,
    bool AutoApprove
) : IRequest<Result<Guid>>;
