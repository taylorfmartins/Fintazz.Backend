namespace Fintazz.Application.RecurringCharges.Commands.ApproveRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Aprova e lança manualmente uma cobrança recorrente pendente, com possibilidade de ajustar o valor.
/// </summary>
/// <param name="RecurringChargeId">ID da cobrança recorrente a ser aprovada.</param>
/// <param name="Amount">Valor a ser lançado. Se não informado, usa o valor cadastrado na recorrente.</param>
public record ApproveRecurringChargeCommand(Guid RecurringChargeId, decimal? Amount) : ICommand<Guid>;
