namespace Fintazz.Application.RecurringCharges.Commands.DeleteRecurringCharge;

using MediatR;
using Fintazz.Domain.Shared;

public record DeleteRecurringChargeCommand(Guid Id) : IRequest<Result>;
