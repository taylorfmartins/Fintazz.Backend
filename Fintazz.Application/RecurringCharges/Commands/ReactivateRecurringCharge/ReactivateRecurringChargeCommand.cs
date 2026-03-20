namespace Fintazz.Application.RecurringCharges.Commands.ReactivateRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Reativa uma cobrança recorrente que estava desativada.
/// </summary>
/// <param name="RecurringChargeId">ID da cobrança recorrente a ser reativada.</param>
public record ReactivateRecurringChargeCommand(Guid RecurringChargeId) : ICommand;
